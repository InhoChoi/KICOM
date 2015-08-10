using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;
using System.Collections;
using System.Threading;

namespace kicom {
    class FaceAnalysis {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("e6edd17d1bbd4ca69d14ccf572e9af20");
        private FileManagement fileMangemnet = null;
        private DBManagement dbManagement = null;
        private Person[] persons = null;
        private System.Timers.Timer aTimer = null;
        private Semaphore mutex = null;
        private XMLwriter xmLwriterInstance = null;


        public FaceAnalysis(string folderPath) {
            this.fileMangemnet = new FileManagement(folderPath);
            this.dbManagement = new DBManagement(folderPath);
            this.mutex = new Semaphore(1, 1);

            this.aTimer = new System.Timers.Timer(10000);
            this.aTimer.Elapsed += aTimer_Elapsed;

            //The face ID will expire 24 hours after detection.
            this.aTimer.Interval = 24 * 60 * 60 * 1000;
            this.aTimer.Enabled = true;

            // xmlwriter를 인스턴스화
            xmLwriterInstance = XMLwriter.GetInstance();

            init();
        }

        //타이머 Elapsed 함수
        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            init();
        }

        //Person의 초기화 함수
        private async void init() {
            //Mutext Wait
            mutex.WaitOne();
            this.persons = await this.getFacesFromDB();

            //Mutext Release
            mutex.Release();
        }

        // 사진 이미지를 이용하여 Face 정보 획득
        private async Task<Face[]> UploadAndDetectFaces(string imageFilePath) {
            try {
                using (Stream imageFileStream = File.OpenRead(imageFilePath)) {
                    Face[] faces = await faceServiceClient.DetectAsync(imageFileStream);

                    return faces;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return new Face[0];
            }

        }

        //저장소의 이미지파일과 DB의 내용을 이용하여 Person 작성
        public async Task<Person[]> getFacesFromDB() {
            Person[] persons = dbManagement.select();

            foreach (Person person in persons) {
                if (fileMangemnet.exists(person.imgname))
                {
                    string filepath = fileMangemnet.getFilePath(person.imgname);
                    Face[] faces = await this.UploadAndDetectFaces(filepath);
                    if (faces.Length == 1)
                    {
                        person.faceid = faces[0].FaceId.ToString();
                    }
                }
            }
            return persons;
        }

        //저장소에 이미지 저장 및 DB 저장
        public async void register(string name, string filepath, string realtion, string etc) {
            if (System.IO.File.Exists(filepath)) {
                Face[] faces = await this.UploadAndDetectFaces(filepath);
                if (faces.Length == 1) {
                    string filename = Path.GetFileName(filepath);
                    fileMangemnet.copyFrom(filepath);
                    Console.WriteLine(filename);
                    Person person = new Person(name, filename, realtion, etc);

                    dbManagement.insert(person);
                }
                else {
                    throw new Exception("사진속의 얼굴이 하나이상이거나 없습니다");
                }
            }
            else {
                throw new Exception(filepath + " 파일이 존재하지 않습니다");
            }
        }

        // 저장소에 등록된 사람들인지 아닌지 확인
        public async void verify(VisitorInfo info) {

            if (this.persons == null)
                throw new Exception("Person 객체가 생성되지 않았습니다");

            //Mutex Wait
            this.mutex.WaitOne();

            string filepath = info.filepath;
            //List<Result> ret = new List<Result>();
            Face[] faces = await this.UploadAndDetectFaces(filepath);


            List<Result> results = new List<Result>();

            //얼굴이 존재하지 않을 경우
            if (faces.Length == 0) {
                //Mutext Relase
                this.mutex.Release();

                Result result = new Result("No face", filepath, "No face");
                results.Add(result);
                xmLwriterInstance.HistoryWriting(results.ToArray());
                xmLwriterInstance.ResultsXmlWriting(results.ToArray());

                //xmLwriterInstance.pushXMLQueue(result);
                return;
            }
            

            Boolean verifed = false;

            // DB에 사람들과 비교하는 부분
            foreach (Face face in faces) {
                foreach (Person person in this.persons) {
                    if (person.faceid != null) {
                        Guid guid = new Guid(person.faceid);
                        VerifyResult verifyresult = await faceServiceClient.VerifyAsync(face.FaceId, guid);

                        //DB에 저장한 사람들과 일치한 경우
                        if (verifyresult.IsIdentical) {
                            Result result = new Result(person.name, filepath, person.relation);

                            verifed = true;
                            results.Add(result);
                        }
                    }
                }
                if (verifed == false) {
                    Result result = new Result("Unknown", filepath, "Unknown");
                    results.Add(result);
                }
                else {
                    verifed = true;
                }
            }

            xmLwriterInstance.HistoryWriting(results.ToArray());
            xmLwriterInstance.ResultsXmlWriting(results.ToArray());

            //Mutext Relase
            this.mutex.Release();
        }

    }
}

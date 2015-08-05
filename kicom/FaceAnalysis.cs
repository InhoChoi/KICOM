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

namespace kicom
{
    class FaceAnalysis
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("9ae4347d4a2d4da680dca24eec1f742c");
        private FileManagement fileMangemnet = null;
        private DBManagement dbManagement = null;
        private Person[] persons = null;
        private System.Timers.Timer aTimer = null;
        private Semaphore mutex = null;

        public FaceAnalysis(string folderPath)
        {
            this.fileMangemnet = new FileManagement(folderPath);
            this.dbManagement = new DBManagement(folderPath);
            init();

            this.aTimer = new System.Timers.Timer(10000);
            this.aTimer.Elapsed += aTimer_Elapsed;
            this.aTimer.Interval = 30 * 60 * 1000;
            this.aTimer.Enabled = true;

            this.mutex = new Semaphore(1, 1);
        }

        //타이머 Elapsed 함수
        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Mutext Wait
            mutex.WaitOne();
            init();

            //Mutext Release
            mutex.Release();
        }

        //Person의 초기화 함수
        private async void init()
        {
            this.persons = await this.getFacesFromDB();
        }

        // 사진 이미지를 이용하여 Face 정보 획득
        private async Task<Face[]> UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    Face[] faces = await faceServiceClient.DetectAsync(imageFileStream);

                    return faces;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new Face[0];
            }

        }

        //저장소의 이미지파일과 DB의 내용을 이용하여 Person 작성
        public async Task<Person[]> getFacesFromDB()
        {
            Person[] persons = dbManagement.select();

            foreach (Person person in persons)
            {
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
        public async void register(string name, string filepath, string realtion, string etc)
        {
            if (System.IO.File.Exists(filepath))
            {
                Face[] faces = await this.UploadAndDetectFaces(filepath);
                if (faces.Length == 1)
                {
                    string filename = Path.GetFileName(filepath);
                    fileMangemnet.copyFrom(filepath);
                    Console.WriteLine(filename);
                    Person person = new Person(name, filename, realtion, etc);

                    dbManagement.insert(person);
                }
                else 
                {
                    throw new Exception("사진속의 얼굴이 하나이상이거나 없습니다");
                }
            }
            else
            {
                throw new Exception(filepath + " 파일이 존재하지 않습니다");
            }
        }

        // 저장소에 등록된 사람들인지 아닌지 확인
        public async Task<Result[]> verify(VisitorInfo info)
        {

            if (this.persons == null)
                throw new Exception("Person 객체가 생성되지 않았습니다");

            //Mutex Wait
            this.mutex.WaitOne();

            string filepath = info.filepath;

            List<Result> ret = new List<Result>();
            Face[] faces = await this.UploadAndDetectFaces(filepath);
            //Person[] persons = await this.getFacesFromDB();

            foreach (Face face in faces)
            {
                foreach (Person person in this.persons)
                {
                    if (person.faceid != null)
                    {
                        Guid guid = new Guid(person.faceid);
                        VerifyResult verifyresult = await faceServiceClient.VerifyAsync(face.FaceId, guid);
                        if (verifyresult.IsIdentical)
                        {
                            string imgpath = fileMangemnet.getFilePath(person.imgname);
                            Result result = new Result(person.name, imgpath, person.relation);
                            ret.Add(result);
                        }

                    }
                }
            }

            //Mutext Relase
            this.mutex.Release();

            return ret.ToArray();   
        }

    }
}

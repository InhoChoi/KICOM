using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kicom {
    class FileManagement {
        private const string folderName = "images";
        private string folderPath = null;

        //생성자
        public FileManagement() {
            this.folderPath = @"./images";
            this.createFolder(this.folderPath);
        }

        //생성자 - folder path가 있는 경우
        public FileManagement(string folderPath) {
            this.folderPath = Path.Combine(folderPath, folderName);
            this.createFolder(this.folderPath);
        }

        //Folder 생성
        private void createFolder(string folderPath) {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            //폴더 유무 확인후 폴더 생성
            if (di.Exists == false) {
                di.Create();
            }
        }

        //파일 주소 획득 함수
        public string getFilePath(string name) {
            string filepath = Path.Combine(folderPath, name);

            //파일 존재 유무 확인
            if (System.IO.File.Exists(filepath)) {
                return filepath;
            }
            else {
                throw new Exception(name + " isn't existed");
            }
        }

        //저장소에 들어오는 파일 리스트 출력
        public string[] getFileList() {
            string[] files = System.IO.Directory.GetFiles(folderPath);
            List<string> ret = new List<string>();
            foreach(string file in files) {
                ret.Add(Path.GetFileName(file));
            }

            return ret.ToArray();
        }

        //파일 삭제
        public void delete(string filename) {
            string filepath = Path.Combine(folderPath, filename);

            //파일 존재 유무 확인후 삭제
            if (System.IO.File.Exists(filepath)) {
                try {
                    System.IO.File.Delete(filepath);
                }
                catch (System.IO.IOException err) {
                    Console.WriteLine(err.Message);
                    return;
                }
            }
        }

        //모든 파일 삭제
        public void deleteAll() {
            string[] files = this.getFileList();
            foreach (string file in files) {
                delete(file);
            }
        }

        //저장소로 파일 복사
        public void copyFrom(string filepath,string fileName) {
            try {
                string destFile = System.IO.Path.Combine(folderPath, fileName);
                System.IO.File.Copy(filepath, destFile, true);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        //저장소에서 파일복사
        public void copyTo(string filename,string destFolder) {
            try {
                string filepath = Path.Combine(this.folderPath, filename);
                string destFile = System.IO.Path.Combine(destFolder, filename);
                System.IO.File.Copy(filepath, destFile, true);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        //파일존재유무
        public Boolean exists(string filename) {
            string filepath = Path.Combine(folderName,filename);
            return System.IO.File.Exists(filepath);
        }
    }
}

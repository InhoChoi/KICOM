using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace kicom {
    class ArduinoSerial {
        /// <summary>
        /// Serial 관련 변수
        /// private ardSerialPort
        /// </summary>
        private SerialPort ardSerialPort = new SerialPort();

        /// <summary>
        /// Araduino Serial 생성자
        /// 아두이노를 찾고 연결, 하드웨어가 존재하지 않으면 Exception 오류를 내뽑는다.
        /// </summary>
        public ArduinoSerial() {
            Boolean isOpend = false;
            string[] portNames = SerialPort.GetPortNames();
            //모든 Serial Port을 가져와서 아두이노와 연결작업
            foreach (string name in portNames) {
                ardSerialPort.PortName = name;
                ardSerialPort.BaudRate = 9600;
                try {
                    ardSerialPort.Open();
                    // 아두이노 에코로 판별
                    ardSerialPort.WriteLine("open");
                    if (ardSerialPort.ReadLine().ToString().StartsWith("open")) {
                        this.write(" ");
                        isOpend = true;
                        break;
                    }
                    else {
                        ardSerialPort.Close();
                    }
                }
                catch (Exception e) {

                }
            }
            //Open이 되지 않았을 겨우
            if (!isOpend) {
                throw new Exception("연결된 하드웨어가 존재하지 않습니다");
            }
        }
        /// <summary>
        /// Serial 통신을 통해 아두이노로 문자를 보낸다
        /// </summary>
        /// <param name="input">보낼 문자</param>
        public void write(string input) {
            if (input != "" && input.Length <= 32) {
                string text = input + "\n";
                ardSerialPort.Write(text);
            }
        }

        public Boolean check() {
            try {
                ardSerialPort.WriteLine("open");
                if (ardSerialPort.ReadLine().ToString().StartsWith("open")) {
                    this.write(" ");
                    return true;
                }
            }
            catch (Exception err) {

            }
            return false;
        }

        public bool isOpen() {
            return ardSerialPort.IsOpen;
        }

    }
    ///
    class FileTrigger {
        private FileSystemWatcher watcher = null;
        private ArduinoSerial serial = null;
        private string message = null;
        private string fileName = null;
        /// <summary>
        /// FileTrigger 생성자
        /// </summary>
        /// <param name="path">파일을 감지할 폴더</param>
        /// <param name="fileName">감지할 파일 이름</param>
        /// <param name="serial">아두이노 시리얼 객체</param>
        [IODescriptionAttribute("FileSystemWatcherDesc")]
        [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        public FileTrigger(string path, string fileName, ArduinoSerial serial) {
            this.serial = serial;

            this.fileName = fileName;

            this.watcher = new FileSystemWatcher();
            this.watcher.Path = path;
            this.watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.watcher.Filter = "*.txt";

            this.watcher.Created += watcher_Created;
            this.watcher.Changed += watcher_Created;

            this.watcher.EnableRaisingEvents = true;
            Console.WriteLine("Start File Trigger");
        }

        /*
        public FileTrigger(string path)
        {
            this.serial = new ArduinoSerial();

            this.watcher = new FileSystemWatcher();
            this.watcher.Path = path;
            this.watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.watcher.Filter = "*.txt";

            this.watcher.Created += watcher_Created;
        }*/

        /// <summary>
        /// Close() 매서드
        /// </summary>
        public void Close() {
            watcher.Dispose();
        }
        /// <summary>
        /// 파일 생성시 될 경우의 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watcher_Created(object sender, FileSystemEventArgs e) {
            string filepath = e.FullPath;
            string fileName = Path.GetFileName(filepath);
            if (this.fileName != fileName) {
                return;
            }


            //다른 프로세서가 사용하고 있을 경우에는 계속해서 기다린후에 파일 오픈
            int count = 0;
            while (true) {
                try {
                    using (StreamReader sr = File.OpenText(filepath)) {
                        string s = "";
                        while ((s = sr.ReadLine()) != null) {
                            message = s;
                        }
                        serial.write(message);
                    }
                    break;

                }
                catch (Exception err) {
                    //10번이상 읽어들이지 않을 경우 break
                    count++;
                    if (count > 10) {
                        MessageBox.Show("아두이노 하드웨어 연결을 확인후 프로그램을 다시 시작해주세요", "Error");
                        break;
                    }
                }
            }

        }
    }
}

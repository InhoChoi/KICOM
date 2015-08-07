using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arduino1
{
    class ArduinoSerial
    {
        private SerialPort ardSerialPort = new SerialPort();

        public ArduinoSerial()
        {
            ardSerialPort.PortName = "COM3";
            ardSerialPort.BaudRate = 9600;
            ardSerialPort.Open();
        }

        public void write(string input)
        {
            if (input != "")
            {
                string text = input + "\n";
                ardSerialPort.Write(text);
            }
        }
    }

    class FileTrigger
    {
        private FileSystemWatcher watcher = null;
        private ArduinoSerial serial = null;
        private string message = null;

        [IODescriptionAttribute("FileSystemWatcherDesc")]
        [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        public FileTrigger(string path, ArduinoSerial serial)
        {
            this.serial = serial;

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


        public void Close()
        {
            watcher.Dispose();
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            string filepath = e.FullPath;
            Console.WriteLine("File Created " + e.FullPath);

            //다른 프로세서가 사용하고 있을 경우에는 계속해서 기다린후에 파일 오픈
            while (true)
            {
                try
                {
                    using (StreamReader sr = File.OpenText(filepath))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            message = s;
                        }
                        serial.write(message);
                    }
                    break;

                }
                catch (Exception err)
                {
                }
            }

        }
    }
}

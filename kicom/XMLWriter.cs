using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kicom {
    public class XMLwriter {

        //private static Singleton _uniqueInstance;
        private static XMLwriter _uniqueInstance = new XMLwriter();
<<<<<<< HEAD
        private static Queue<Result> writerQueue = new Queue<Result>();
=======
        private static Queue<Result> history = new Queue<Result>();
>>>>>>> NewXmlFunction

        private XMLwriter() {
            this.ReadingHistory();
        }

        public static XMLwriter GetInstance() {
            return _uniqueInstance;
        }

<<<<<<< HEAD
        private async Task OverWriteXml() {
            while (true) {
                //var result = await OverWriting();
                //await Task.Delay(1000);
            }
        }

        public bool pushXMLQueue(Result _data) {
            if (writerQueue == null) return false;
            writerQueue.Enqueue(_data);
            OverWriting();
            return true;
        }

        public bool OverWriting() {
        //public async Task<bool> OverWriting() {
            if (!writerQueue.Any()) return false;

            using (XmlTextWriter writer = new XmlTextWriter("broadcast.xml", System.Text.Encoding.UTF8)) {
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("Info");
                writer.WriteStartElement("Name");
                writer.WriteString(writerQueue.Peek().name);
                writer.WriteEndElement();
                writer.WriteStartElement("Relation");
                writer.WriteString(writerQueue.Peek().relation);
                writer.WriteEndElement();
                writer.WriteStartElement("Photo_Path");
                writer.WriteString(writerQueue.Peek().filepath);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Close();

                writerQueue.Dequeue();

                return true;
            }
        }

        //Result가 여러개 있을 경우의 Xml Writer
        public void XmlWrting(Result[] results) {
            using (XmlTextWriter writer = new XmlTextWriter("broadcast.xml", System.Text.Encoding.UTF8)) {
=======
        //Result가 여러개 있을 경우의 Xml Writer
        public void AlertWriting(Result[] results) {
            using (XmlTextWriter writer = new XmlTextWriter("../../../Web/static/xml/broadcast.xml", System.Text.Encoding.UTF8)) {
>>>>>>> NewXmlFunction
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                foreach (Result result in results) {
                    writer.WriteStartElement("Info");
                    writer.WriteStartElement("Name");
                    writer.WriteString(result.name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Relation");
                    writer.WriteString(result.relation);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Photo_Path");
                    writer.WriteString(result.filepath);
                    writer.WriteEndElement();
<<<<<<< HEAD
=======
                }
                writer.WriteEndElement();
                writer.Close();
            }
        }

        private void ReadingHistory() {
            try {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load("history.xml");
                XmlElement root = xmldoc.DocumentElement;


                //XmlNodeList를 쓰게 되면 해당 노드를 선택합니다. 
                XmlNodeList xnList = root.SelectNodes("/History/Info");

                List<Result> results = new List<Result>(); 

                // 노드 요소의 값을 읽어 옵니다.
                foreach (XmlNode node in xnList) {
                    string Name = node["Name"].InnerText;
                    string Relation = node["Relation"].InnerText;
                    string PhotoPath = node["Photo_Path"].InnerText;
                    string Date = node["Date"].InnerText;

                    Result result = new Result(Name, Relation, PhotoPath, Date);
                    results.Add(result);
                }
                foreach (Result ru in Enumerable.Reverse(results.ToArray())) {
                    history.Enqueue(ru);
                }

            }
            catch (Exception ex) {
            }
        } 

        public void HistoryWriting(Result[] results) {
            foreach (Result result in results) {
                history.Enqueue(result);
            }
            //20개 갯수맞추기
            while (true) {
                if (history.Count <= 20) {
                    break;
                }
                history.Dequeue();
            }
            using (XmlTextWriter writer = new XmlTextWriter("../../../Web/static/xml/history.xml", System.Text.Encoding.UTF8)) {
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("History");
                foreach (Result result in Enumerable.Reverse(history)) {
                    writer.WriteStartElement("Info");
                    writer.WriteElementString("Name", result.name);
                    writer.WriteElementString("Relation", result.relation);
                    writer.WriteElementString("Photo_Path", result.filepath);
                    if (result.date == null) {
                        writer.WriteElementString("Date", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
                    }
                    else {
                        writer.WriteElementString("Date", System.DateTime.Now.ToString(result.date));
                    }
>>>>>>> NewXmlFunction
                    writer.WriteEndElement();
                }
                writer.Close();
            }
        }
    }
}

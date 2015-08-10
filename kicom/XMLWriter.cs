using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kicom {
    public class XMLwriter {

        //private static Singleton _uniqueInstance;
        private static XMLwriter _uniqueInstance = new XMLwriter();
        private static Queue<Result> writerQueue = new Queue<Result>();

        private XMLwriter() {
            //OverWriteXml();
        }

        public static XMLwriter GetInstance() {
            return _uniqueInstance;
        }

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
        public void ResultsXmlWriting(Result[] results) {
            using (XmlTextWriter writer = new XmlTextWriter("broadcast.xml", System.Text.Encoding.UTF8)) {
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("Result");
                foreach (Result result in results) {
                    writer.WriteStartElement("Info");
                    writer.WriteElementString("Name",result.name);
                    writer.WriteElementString("Relation",result.relation);
                    writer.WriteElementString("Photo_Path",result.filepath);
                    writer.WriteElementString("Date", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
        }
    }
}

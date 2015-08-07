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
            overWriteXML();
        }

        public static XMLwriter GetInstance() {
            return _uniqueInstance;
        }

        private async Task overWriteXML() {
            while (true)
            {
                var result = await OverWriting();
                await Task.Delay(1000);
            }
        }

        public static bool pushXMLQueue(Result _data) {
            if (writerQueue != null) {
                writerQueue.Enqueue(_data);
                return true;
            }
            return false;
        }


        private async Task<bool> OverWriting() {
            if (writerQueue.Any()) {
                using (XmlTextWriter writer = new XmlTextWriter("broadcast.xml", System.Text.Encoding.UTF8)) {
                    writer.WriteStartDocument(true);
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
                    writer.WriteStartDocument();
                    writer.Close();

                    writerQueue.Dequeue();

                    return true;
                }
            }
            return false;
        }
    }
}

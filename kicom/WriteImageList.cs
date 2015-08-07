using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kicom {
    public class WriteImageList {
        private static WriteImageList UniqueInstance = new WriteImageList();
        private static Queue<string> ListingQueue = new Queue<string>();

        private WriteImageList() {
            FileInfo _fileinfo = new FileInfo("ImageData.xml");

            if (!_fileinfo.Exists) {
                using (XmlTextWriter writer = new XmlTextWriter("ImageData.xml", System.Text.Encoding.UTF8)) {
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    //writer.Indentation = 2;
                    writer.WriteStartElement("Images");
                    writer.WriteStartElement("Image");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Close();
                }
            }

            addImageListTask();

        }

        public static WriteImageList GetInstance() {
            return UniqueInstance;
        }

        private async Task addImageListTask() {
            while (true) {
                var result = await OverWriting();
                await Task.Delay(1000);
            }
        }

        public bool pushList(string _data) {
            if (ListingQueue != null) {
                ListingQueue.Enqueue(_data);
                return true;
            }
            return false;
        }


        private async Task<bool> OverWriting() {
            if (ListingQueue.Any()) {
                    XmlDocument xmlDoc;
                    XmlElement xmlEle;
                    XmlNode newNode;

                    xmlDoc = new XmlDocument();
                    xmlDoc.Load("ImageData.xml"); // XML문서 로딩

                    newNode = xmlDoc.SelectSingleNode("Images/Image"); // 추가할 부모 Node 찾기

                    xmlEle = xmlDoc.CreateElement("ImagePath"); // 추가할 Node 생성

                    newNode.AppendChild(xmlEle); // 위에서 찾은 부모 Node에 자식 노드로 추가..

                    xmlDoc.Save("ImageData.xml"); // XML문서 저장..
                    xmlDoc = null;

                    return true;
            }
            return false;
        }
    }
}

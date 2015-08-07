using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kicom {
    public class WriteImageList {
        private static WriteImageList _uniqueInstance = new WriteImageList();
        private static Queue<string> writerQueue = new Queue<string>();

        private WriteImageList() {
            XmlTextWriter maker = new XmlTextWriter("Imagedata.xml", System.Text.Encoding.UTF8);
            maker.Close();
        }

        public static WriteImageList GetInstance() {
            return _uniqueInstance;
        }

        private async Task addImageListTask() {
            while (true) {
                var result = await OverWriting();
                await Task.Delay(1000);
            }
        }

        public static bool pushList(string _data) {
            if (writerQueue != null) {
                writerQueue.Enqueue(_data);
                return true;
            }
            return false;
        }


        private async Task<bool> OverWriting() {
            if (writerQueue.Any()) {
                    XmlDocument xmlDoc;
                    XmlElement xmlEle;
                    XmlNode newNode;

                    xmlDoc = new XmlDocument();
                    xmlDoc.Load("Imagedata.xml"); // XML문서 로딩

                    newNode = xmlDoc.SelectSingleNode("Image/Images"); // 추가할 부모 Node 찾기

                    xmlEle = xmlDoc.CreateElement("ImagePath"); // 추가할 Node 생성

                    newNode.AppendChild(xmlEle); // 위에서 찾은 부모 Node에 자식 노드로 추가..

                    xmlDoc.Save("Imagedata.xml"); // XML문서 저장..
                    xmlDoc = null;

                    return true;
            }
            return false;
        }
    }
}

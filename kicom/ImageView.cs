using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace kicom {
    public static class ImageView {
        #region Property
        private static long ? _ImageCount;
        public static long ? ImageCount {
            get {
                try {
                    XDocument XDoc = XDocument.Load("ImageData.xml");

                    _ImageCount = XDoc.Descendants("ImagePath").Count();

                    return _ImageCount;
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message); 
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Image From XML Data
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static List<ImageEntity> GetAllImageData() {

            // Load Xml Document
            XDocument XDoc = XDocument.Load("ImageData.xml");

            // Query for retriving all Images data from XML
            var Query = (from Q in XDoc.Descendants("Image")
                select new ImageEntity {
                    ImagePath = Q.Element("ImagePath").Value
                });
            return Query.ToList<ImageEntity>();
            
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace kicom {
    public static class ImageView {
        #region Methods

        public static List<ImageEntity> GetAllImagesData() {
            try {
                // Load Xml Document
                XDocument XDoc = XDocument.Load("ImageData.xml");

                // Query for retriving all Images data from XML
                var Query = from Q in XDoc.Descendants("Image")
                            select new ImageEntity {
                                
                                ImagePath = Q.Element("ImagePath").Value
                            };

                // return images data
                return Query.ToList<ImageEntity>();

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}

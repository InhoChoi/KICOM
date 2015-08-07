using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kicom.Pages
{
    /// <summary>
    /// History.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class History : UserControl
    {
        public History()
        {
            this.InitializeComponent();

            try
            {
                BindImages(); // Bind Image in Constructor
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        /// <summary>
        /// Bind Image in List Box Control
        /// </summary>
        private void BindImages()
        {
            try
            {
                // Store Data in List Object
                List<ImageEntity> ListImageObj = ImageView.GetAllImagesData();

                // Check List Object Count
                if (ListImageObj.Count > 0)
                {
                    // Bind Data in List Box Control.
                    LsImageGallery.DataContext = ListImageObj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

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

namespace kicom {
    /// <summary>
    /// History.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class History : UserControl {

        public void refreshImageList() {
            
            lbImageGallery.DataContext = null;
            List<ImageEntity> ListImageEntityObj = ImageView.GetAllImageData();

            if (ListImageEntityObj != null) {
                if (ListImageEntityObj.Count > 0) {
                    Console.WriteLine(ListImageEntityObj.Count);
                    lbImageGallery.DataContext = ListImageEntityObj;
                }
            }
            
        }

        public History() {
            InitializeComponent();

            refreshImageList();
        }
    }
}

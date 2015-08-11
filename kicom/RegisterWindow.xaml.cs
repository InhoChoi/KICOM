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
using System.Windows.Shapes;

namespace kicom {
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : UserControl {
        public FaceAnalysis faceAnyalysis = null;
        private string filePath = null;

        public RegisterWindow() {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e) {
            var openDlg = new Microsoft.Win32.OpenFileDialog();

            openDlg.Filter = "JPEG Image(*.jpg)|*.jpg";
            bool? result = openDlg.ShowDialog(Application.Current.MainWindow);

            if (!(bool)result) {
                return;
            }

            filePath = openDlg.FileName;

            Uri fileUri = new Uri(filePath);
            BitmapImage bitmapSource = new BitmapImage();

            bitmapSource.BeginInit();
            bitmapSource.CacheOption = BitmapCacheOption.None;
            bitmapSource.UriSource = fileUri;
            bitmapSource.EndInit();

            FacePhoto.Source = bitmapSource;
        }

        private async void Register_Click(object sender, RoutedEventArgs e) {
            if (faceAnyalysis == null) {
                Console.Write("null!");
                return;
            }

            if (this.Name_tb.Text == "" || this.Relation_tb.Text == "") {
                MessageBox.Show("Must to fill the text block name and relation", "Error");
                return;
            }

            int result = 0;
            try {
               result = await faceAnyalysis.register(Name_tb.Text, this.filePath, Relation_tb.Text, ETC_tb.Text);
            }
            catch(Exception err) {
                MessageBox.Show(err.Message, "Error");
                return;
            }


            MessageBox.Show("Face is registered", "Success!");

        }

    }
}

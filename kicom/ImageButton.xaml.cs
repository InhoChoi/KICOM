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
    /// ImageButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageButton {
        public ImageSource mImgOver;
        public ImageSource ImgOver {
            get { return mImgOver; }
            set {
                mImgOver = value;
                Update();
            }
        }

        public ImageSource mImgDown;
        public ImageSource ImgDown {
            get { return mImgDown; }
            set {
                mImgDown = value;
                Update();
            }
        }

        public ImageSource mImgNormal;
        public ImageSource imgNormal {
            get { return mImgNormal; }
            set {
                mImgNormal = value;
                Update();
            }
        }

        public ImageSource mImgDisabled;
        public ImageSource ImgDisabled {
            get { return mImgDisabled; }
            set {
                mImgDisabled = value;
                Update();
            }
        }

        private void ImageButton_Loaded(object sender, RoutedEventArgs e) {
            Update();
        }

        private void ImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
            SetImgEnabled();
        }

        private void Update() {
            this.Resources["imgOver"] = ImgOver;
            this.Resources["imgDown"] = ImgDown;
            SetImgEnabled();
        }

        private void SetImgEnabled() {
            if (IsEnabled) {
                this.Background = new ImageBrush(imgNormal);
            }
            else {
                this.Background = new ImageBrush(ImgDisabled); 
            }
        }

        public ImageButton() {
            IsEnabledChanged += ImageButton_IsEnabledChanged;
            Loaded += ImageButton_Loaded;
        }
    }
}

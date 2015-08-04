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

    // 얼굴의 기본적인 해석 정보를 넘기기위한 클래스
    public class VisitorSimpleData {
        public bool isSuspicious;
        public string photoPath;

        public VisitorSimpleData(bool _isSuspicious, string _photoPath) {
            isSuspicious = _isSuspicious;
            photoPath = _photoPath;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
    }
}

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

        // 사용할 변수
        private bool needSnapShot = false;
        private int dumpCount = 0;
        private int maxBodyCount = 0;
        private int preBodyCount = 0;
        private int snapShotWait = 7;
        private string historyFolderPath;
        private string logFolderPath;
        private bool isSuspicious = true;

        // 1. 키넥트 센서
        private KinectSensor kinect = null;

        // 2. 컬러 프레임을 핸들링
        private ColorFrameReader CFReader = null;
        private WriteableBitmap colorBitmap = null;


        // 3. 바디 프레임을 핸들링
        private BodyFrameReader BFReader = null;
        private Body[] bodies = null;

        /// <summary>
        /// 생성자 - 초기 변수 & 객체 초기화
        /// </summary>
        public MainWindow() {
            
            // 필요 변수 초기화
            historyFolderPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\History\"; 
            logFolderPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Log\log.txt";

            stringToLog("hahah");
            Console.WriteLine(historyFolderPath);
            // Get Default KinectSensor
            this.kinect = KinectSensor.GetDefault();

            // for Handling Color Frame
            if (kinect != null) {
                this.CFReader = kinect.ColorFrameSource.OpenReader();

                // 이벤트 핸들러 넘겨줌
                this.CFReader.FrameArrived += this.SaveColorMap;
                FrameDescription colorFrameDescription =
                    this.kinect.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
                colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
                
                // for Handling Body Frame
                this.BFReader = kinect.BodyFrameSource.OpenReader();
                this.maxBodyCount = this.kinect.BodyFrameSource.BodyCount;

                // 이벤트 핸들러 넘겨줌
                this.BFReader.FrameArrived += this.ObserveDoor;
                this.bodies = new Body[this.maxBodyCount];

                // for Handling Face Frame

                this.kinect.Open();
            }
            this.DataContext = this;
            InitializeComponent();
        }
    }
}

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
using Microsoft.Kinect;
using System.IO;
using System.Globalization;
using System.ComponentModel;

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


        /// <summary>
        /// Observe Function - Always
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObserveDoor(object sender, BodyFrameArrivedEventArgs e) {
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame()) {

                // When have a Body Frame
                if (bodyFrame != null) {

                    // Re Loading Body Data
                    bodyFrame.GetAndRefreshBodyData(this.bodies);

                    if (this.bodies == null) {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // compare pre-frame and current-frame
                    this.dumpCount = 0;

                    foreach (Body t in bodies) {
                        if (t.IsTracked) { this.dumpCount++; }
                    }

                    if (this.dumpCount > this.preBodyCount && !this.needSnapShot) {
                        this.needSnapShot = true;
                    }

                    this.preBodyCount = this.dumpCount;

                }
            }

        }
        /// <summary>
        /// 컬러 프래임 데이터를 비트맵으로 전환 [Save Color Frame to bitmap(jpeg)]
        /// </summary>
        /// <param name="slender"></param>
        /// <param name="e"></param>
        private void SaveColorMap(object slender, ColorFrameArrivedEventArgs e) {

            // 스냅샷을 찍어야 할 때만 컬러 프래임의 정보를 가져옴
            if (this.needSnapShot) {
                try {
                    if (this.needSnapShot) {
                        if (this.snapShotWait != 0) {
                            this.snapShotWait--;
                        }
                        else {
                            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) {
                                if (colorFrame != null) {
                                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer()) {

                                        this.colorBitmap.Lock();

                                        // verify data and write the new color frame data to the display bitmap
                                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight)) {
                                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                                this.colorBitmap.BackBuffer,
                                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                                ColorImageFormat.Bgra);

                                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                                        }

                                        this.colorBitmap.Unlock();
                                    }
                                }
                            }
                            
                            // Jpeg 확장자로 데이터를 인코딩시켜주늩 인코더 객체 생성
                            BitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));

                            string date = DateTime.Now.ToString("yyyy'-'MM'-'dd", CultureInfo.CurrentUICulture.DateTimeFormat);
                            string time = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
                            string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                            // 지정한 히스토리 폴더 패스에 년-월-일-시-분-초 형식으로 저장
                            string path = System.IO.Path.Combine(historyFolderPath, "[" + date + "]_" + time + ".jpeg");
                            string log = string.Format("{0}\t{1}\t{2}", date + time, "스냅샷이 촬영되어 히스토리 파일에 저장 되었습니다.", path);
                            
                            using (FileStream fs = new FileStream(path, FileMode.Create)) {
                                // 이미지 파일 저장
                                encoder.Save(fs);
                                fs.Close();
                                // 로그 작성
                                stringToLog(log);

                                VisitorSimpleData VSD = new VisitorSimpleData(true, path);

                                // ========================================
                                // 오브젝트 반환
                                // ========================================

                                // 변수 초기화
                                this.needSnapShot = false;
                                this.snapShotWait = 7;
                            }
                        }
                    }
                }
                catch (IOException){
                }
            }
        }

        /// <summary>
        /// Basic Function for WPF - When Closing Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, CancelEventArgs e) {

            if (this.CFReader != null) {
                this.CFReader.Dispose();
                this.CFReader = null;
            }

            if (this.BFReader != null) {
                this.BFReader.Dispose();
                this.BFReader = null;
            }

            if (this.kinect != null) {
                this.kinect.Close();
                this.kinect = null;
            }

        }

        /// <summary>
        /// 문자열을 밭아 로그 파일에 로그기록을 작성하는 함수
        /// </summary>
        /// <param name="_string"></param>
        private void stringToLog(string _string) {
            using (FileStream fs = File.Open(logFolderPath, FileMode.Append, FileAccess.Write)) {
                using (StreamWriter logWriter = new StreamWriter(fs)) {
                    logWriter.WriteLine(_string);
                }
            }
        }
    }
}

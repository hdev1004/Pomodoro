using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double scale = 0.5;
        private static int width = 380;
        private static int height = 380;
        private static int pomodoro_width = 340;
        private static int pomodoro_height = 340;
        

        static double second = 600.0;
        private DateTime startTime;
        private ScaleTransform transform;
        ScaleTransform scaleTransform;
        Setting settingWindow;

        double nowTime = 0;
        private void timer_Tick(Object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;
            nowTime = elapsedTime.TotalSeconds;

            if (nowTime < second)
            {
                nowTime = nowTime / second * 2;
            } else
            {
                nowTime = 2.0;

            }
            DrawPie(nowTime);


        }

        private void DrawPie(double time)
        {
            double size = 270 * scale - 20;
            canvas.Children.Clear();
            Rect rect = new Rect(0, 0, size, size);
            Thickness margin = new Thickness((this.Height / 2) - (size / 2));
            canvas.Margin = margin;

            Point point1;
            Point point2;

            RotateTransform rotateTransform = new RotateTransform(time * 180);
            Middle.RenderTransform = rotateTransform;
            

            if(time <= 1.0)
            {
                Path pieSlice1 = this.canvas.AddPieSlice
                (
                 new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                    new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                    1,
                    rect,
                    0,
                    time * Math.PI,
                    false,
                    SweepDirection.Clockwise,
                    out point1,
                    out point2
                );

                pieSlice1.StrokeLineJoin = PenLineJoin.Round;
            } else 
            {
                Path pieSlice1 = this.canvas.AddPieSlice
                (
                 new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                  new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                   1,
                   rect,
                   0,
                   1 * Math.PI,
                   false,
                   SweepDirection.Clockwise,
                   out point1,
                   out point2
                );
                pieSlice1.StrokeLineJoin = PenLineJoin.Round;

                Path pieSlice2 = this.canvas.AddPieSlice
                (
                 new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                    new SolidColorBrush(Color.FromArgb(255, (byte)139, (byte)155, (byte)197)),
                    1,
                    rect,
                    1 * Math.PI,
                    time * Math.PI,
                    false,
                    SweepDirection.Clockwise,
                    out point1,
                    out point2
                );

                pieSlice2.StrokeLineJoin = PenLineJoin.Round;
            }

           

            
        }

        private void Resize()
        {
            this.Width = width * scale;
            this.Height = height * scale;
            Rect_Form.Width = width * scale - 10;
            Rect_Form.Height = height * scale - 10;


            Pomodoro.Width = pomodoro_width * scale;
            Pomodoro.Height = pomodoro_height * scale - 50;

            Middle.Width = 45 * scale;
            Middle.Height = 45 * scale;
            Middle_Stick.Height = 22.5 * scale;
        }
        private void Loaded()
        {
            
            startTime = DateTime.Now;
            settingWindow = new Setting(this);
            DispatcherTimer timer = new DispatcherTimer();    //객체생성
            timer.Interval = TimeSpan.FromMilliseconds(1);    //시간간격 설정
            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가
            timer.Start();
            /*
                        scaleTransform = new ScaleTransform(scale, scale, width * scale / 2, height * scale / 2);
                        Pomodoro_Form.RenderTransform = scaleTransform;*/

            Resize();
        }
        public MainWindow()
        {
            InitializeComponent(); 


            Loaded();
        }

        private void Pomodoro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }



        private void Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();

        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double sliderValue = e.NewValue;
            Console.WriteLine(sliderValue);
            //scale = 0.5;

            this.Width = width * scale;
            this.Height = height * scale;
            Rect_Form.Width = width * scale - 10;
            Rect_Form.Height = height * scale - 10;


            Pomodoro.Width = pomodoro_width * scale;
            Pomodoro.Height = pomodoro_height * scale - 50;


        }

        /**
         Setting 메뉴 버튼 클릭
         */
        private void Setting_Btn_Click(object sender, RoutedEventArgs e)
        {
            settingWindow.Show();
        }
    }
}
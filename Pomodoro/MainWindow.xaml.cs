using System.Net;
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
        private Boolean isWork = true;
        public int workCnt = 2;
        private int nowWork = 0;
        public Boolean isPlay = false;

        private SolidColorBrush timer_color = null;
        private SolidColorBrush rest_color = null;

        public string timer_hash = "#FFF1EEDC";
        public string border_hash = "#FFB3C8CF";
        public string second_hash = "#FFBED7DC";
        public string rest_hash = "#FFFFFFFF";


        public double second = 10.0;
        public double restSecond = 10.0;
        private DateTime startTime;
        private ScaleTransform transform;
        ScaleTransform scaleTransform;
        Setting settingWindow;

        double nowTime = 0;

        public void setTimerColor(SolidColorBrush brush)
        {
            this.timer_color = brush;
        }

        private void timer_Tick(Object sender, EventArgs e)
        {
            if(isWork)
            {
                TimeSpan elapsedTime = DateTime.Now - startTime;
                nowTime = elapsedTime.TotalSeconds;

                if (nowTime < second)
                {
                    nowTime = nowTime / second * 2;
                }
                else
                {
                    nowTime = 2.0;
                }

                DrawPie(nowTime, isWork);

                if (nowTime >= 2.0)
                {
                    isWork = false;
                    startTime = DateTime.Now;
                }
                Console.WriteLine("Work : " + nowTime);
            } else
            {
                TimeSpan elapsedTime = DateTime.Now - startTime;
                nowTime = elapsedTime.TotalSeconds;

                if (nowTime < restSecond)
                {
                    nowTime = nowTime / restSecond * 2;
                }
                else
                {
                    nowTime = 2.0;
                }

                DrawPie(nowTime, isWork);

                if (nowTime >= 2.0)
                {
                    nowWork += 1;
                    if(workCnt <= nowWork)
                    {
                        DispatcherTimer timer = (DispatcherTimer)sender;
                        timer.Stop();
                        EndPaint();
                        isPlay = false;
                    }
                    isWork = true;
                    startTime = DateTime.Now;
                }
                Console.WriteLine("Work : " + nowTime);
            }
           
        }

        private void EndPaint()
        {
            double size = 270 * scale - 20;
            canvas.Children.Clear();
            Rect rect = new Rect(0, 0, size, size);
            Thickness margin = new Thickness((this.Height / 2) - (size / 2));
            canvas.Margin = margin;

            Point point1;
            Point point2;

            Path pieSlice1 = this.canvas.AddPieSlice
                    (
                    Brushes.White,
                        Brushes.White,
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
                   Brushes.White,
                       Brushes.White,
                       1,
                       rect,
                       1 * Math.PI,
                       2 * Math.PI,
                       false,
                       SweepDirection.Clockwise,
                       out point1,
                       out point2
                   );
            pieSlice2.StrokeLineJoin = PenLineJoin.Round;
        }


        private void DrawPie(double time, Boolean isWork)
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

            if (isWork)
            {
                if (time <= 1.0)
                {
                    Path pieSlice1 = this.canvas.AddPieSlice
                    (
                     timer_color,
                        timer_color,
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
                }
                else
                {
                    Path pieSlice1 = this.canvas.AddPieSlice
                    (
                     timer_color,
                      timer_color,
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
                     timer_color,
                        timer_color,
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
            else
            {
                if (time <= 1.0)
                {
                    Path pieSlice1 = this.canvas.AddPieSlice
                    (
                        rest_color,
                        rest_color,
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


                   Path pieSlice2 = this.canvas.AddPieSlice
                   (
                       timer_color,
                       timer_color,
                       1,
                       rect,
                       time * Math.PI,
                       1 * Math.PI,
                       false,
                       SweepDirection.Clockwise,
                       out point1,
                       out point2
                   );
                    pieSlice2.StrokeLineJoin = PenLineJoin.Round;

                    Path pieSlice3 = this.canvas.AddPieSlice
                   (
                       timer_color,
                       timer_color,
                       1,
                       rect,
                       1 * Math.PI,
                       2 * Math.PI,
                       false,
                       SweepDirection.Clockwise,
                       out point1,
                       out point2
                   );
                    pieSlice3.StrokeLineJoin = PenLineJoin.Round;
                } else
                {
                    Path pieSlice1 = this.canvas.AddPieSlice
                    (
                        rest_color,
                        rest_color,
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
                        rest_color,
                        rest_color,
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

                    Path pieSlice3 = this.canvas.AddPieSlice
                    (
                        timer_color,
                        timer_color,
                        1,
                        rect,
                        time * Math.PI,
                        2 * Math.PI,
                        false,
                        SweepDirection.Clockwise,
                        out point1,
                        out point2
                    );
                    pieSlice3.StrokeLineJoin = PenLineJoin.Round;
                }
            }
        }

        public void setTimer()
        {
            startTime = DateTime.Now;

        }

        public void startTimer()
        {
            startTime = DateTime.Now;
            DispatcherTimer timer = new DispatcherTimer();    //객체생성
            timer.Interval = TimeSpan.FromMilliseconds(1);    //시간간격 설정
            timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가
            timer.Start();
            
        }

        public void Resize(double scale)
        {
            this.scale = scale;
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
            //타이머 가운데 색상
            Color color = (Color)ColorConverter.ConvertFromString(timer_hash);
            SolidColorBrush brush = new SolidColorBrush(color);
            timer_color = brush;

            color = (Color)ColorConverter.ConvertFromString(rest_hash);
            brush = new SolidColorBrush(color);
            rest_color = brush;

            //타이머 테두리 색상
            color = (Color)ColorConverter.ConvertFromString(border_hash);
            brush = new SolidColorBrush(color);
            Rect_Form.Stroke = brush;

            //타이머 초침 색상
            color = (Color)ColorConverter.ConvertFromString(second_hash);
            brush = new SolidColorBrush(color);
            Middle_Stick.Fill = brush;

            settingWindow = new Setting(this);

            /*
                        scaleTransform = new ScaleTransform(scale, scale, width * scale / 2, height * scale / 2);
                        Pomodoro_Form.RenderTransform = scaleTransform;*/

            Resize(scale);
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

        private void Player_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            Player.Fill = new SolidColorBrush(Color.FromRgb(220, 220, 220));
        }

        private void Player_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            Player.Fill = Brushes.White;
        }

        private void Player_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(!isPlay)
            {
                isPlay = true;
                startTimer();
            }
        }
    }
}
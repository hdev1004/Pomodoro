using MaterialDesignColors;
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
using MaterialDesignThemes.Wpf;
using static Pomodoro.JsonManager;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Shell;

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
        public Boolean isPause = false;
        private TimeSpan diffTime = TimeSpan.Zero;
        DispatcherTimer timer;

        private SolidColorBrush timer_color = null;
        private SolidColorBrush rest_color = null;

        public string timer_hash = "#FFF1EEDC";
        public string border_hash = "#FFB3C8CF";
        public string second_hash = "#FFBED7DC";
        public string rest_hash = "#FFFFFFFF";
        private JsonManager jsonManager;

        public double second = 600.0;
        public double restSecond = 30.0;
        private DateTime startTime;
        private DateTime pauseTime;
        Setting settingWindow;

        TaskbarItemInfo taskbarInfo = new TaskbarItemInfo();
        double nowTime = 0;

        public void setTimerColor(SolidColorBrush brush)
        {
            this.timer_color = brush;
        }

        private void timer_Tick(Object sender, EventArgs e)
        {
            if (isPause)
            {
                diffTime = DateTime.Now - pauseTime;
                Start.Icon = new PackIcon
                {
                    Kind = PackIconKind.TimerPlayOutline,
                };
                Start.Header = "시작";
                return;
            } else
            {
                Start.Icon = new PackIcon
                {
                    Kind = PackIconKind.TimerPauseOutline,
                };
                Start.Header = "일시정지";
            }

            if (isWork) //일하는 시간일 때
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

                    this.Show();
                    this.WindowState = WindowState.Normal;


                    Notification();
                    mediaElement.Source = new Uri("./music/나들이 왔어요.mp3", UriKind.Relative);
                    mediaElement.Play();
                }
                //Console.WriteLine("Work : " + nowTime);
            } else //쉬는 시간일때
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
                        timer = (DispatcherTimer)sender;
                        timer.Stop();
                        EndPaint();
                        isPlay = false;

                        diffTime = DateTime.Now - pauseTime;
                        Start.Icon = new PackIcon
                        {
                            Kind = PackIconKind.TimerPlayOutline,
                        };
                        Start.Header = "시작";

                        mediaElement.Source = new Uri("./music/달콤한 휴식 시간.mp3", UriKind.Relative); //끝날 때
                        mediaElement.Play();
                    } else
                    {
                        mediaElement.Source = new Uri("./music/맑은 아침.mp3", UriKind.Relative); //실행 중 일때
                        mediaElement.Play();
                    }
                    Notification();
                    isWork = true;
                    startTime = DateTime.Now;
                }
                //Console.WriteLine("Work : " + nowTime);
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

            System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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

            System.Windows.Shapes.Path pieSlice2 = this.canvas.AddPieSlice
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
                    System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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
                    System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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

                    System.Windows.Shapes.Path pieSlice2 = this.canvas.AddPieSlice
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
                    System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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


                   System.Windows.Shapes.Path pieSlice2 = this.canvas.AddPieSlice
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

                    System.Windows.Shapes.Path pieSlice3 = this.canvas.AddPieSlice
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
                    System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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

                    System.Windows.Shapes.Path pieSlice2 = this.canvas.AddPieSlice
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

                    System.Windows.Shapes.Path pieSlice3 = this.canvas.AddPieSlice
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
            isPause = false;
            isPlay = false;
            Start.Icon = new PackIcon
            {
                Kind = PackIconKind.TimerPlayOutline,
            };
            Start.Header = "시작";

            try
            {
                //타이머 설정
                startTime = DateTime.Now;
                timer.Stop();
            } catch
            {
                Console.WriteLine("타이머가 실행중이 아닙니다.");
            }

            double size = 270 * scale - 20;
            canvas.Children.Clear();
            Rect rect = new Rect(0, 0, size, size);
            Thickness margin = new Thickness((this.Height / 2) - (size / 2));
            canvas.Margin = margin;

            Point point1;
            Point point2;
            Console.WriteLine("4");


            RotateTransform rotateTransform = new RotateTransform(0);
            Middle.RenderTransform = rotateTransform;

            System.Windows.Shapes.Path pieSlice1 = this.canvas.AddPieSlice
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

            System.Windows.Shapes.Path pieSlice2 = this.canvas.AddPieSlice
            (
             rest_color,
                rest_color,
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

        public void startTimer()
        {
            startTime = DateTime.Now;
            timer = new DispatcherTimer();    //객체생성
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

            // 데이터를 JSON 파일로 저장


            settingWindow = new Setting(this);
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

        private void Notification()
        {
            taskbarInfo.ProgressState = TaskbarItemProgressState.Normal; // 작업 표시줄 상태 설정
            // 작업 표시줄 색상 설정
            taskbarInfo.ProgressValue = 1; // 0과 1 사이의 값으로 작업 표시줄의 색상을 변경합니다.
            taskbarInfo.ProgressState = TaskbarItemProgressState.Error; // 주황색 표시줄로 변경
            TaskbarItemInfo = taskbarInfo;
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
            try
            {
                settingWindow.Show();
            } catch (Exception ex)
            {
                Console.WriteLine("창이 닫혔습니다. 새로운 인스터스를 선언합니다.");
                settingWindow = new Setting(this);
                settingWindow.Show();
            }
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

        public void Pause_Start()
        {
            if (!isPlay)
            {
                mediaElement.Stop();
                isPlay = true;
                if (!isPause) //일시정지가 아닐경우
                {
                    isPause = false;
                    startTimer();
                }

            }
            else //실행중이고 일시정지 누를떄
            {
                isPause = !isPause;
                if (isPause) //일시정지일떄
                {
                    pauseTime = DateTime.Now;
                }
                else //다시시작
                {
                    startTime += diffTime;
                }
            }
        }
        //다시시작, 일시정지
        private void Player_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pause_Start();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Pause_Start();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            setTimer();


        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void top_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }

        //작업 표시줄 알람
        private void Window_Activated(object sender, EventArgs e)
        {
            taskbarInfo.ProgressState = TaskbarItemProgressState.Normal; // 작업 표시줄 상태 설정
            // 작업 표시줄 색상 설정
            taskbarInfo.ProgressValue = 0; // 0과 1 사이의 값으로 작업 표시줄의 색상을 변경합니다.
            taskbarInfo.ProgressState = TaskbarItemProgressState.Error; // 주황색 표시줄로 변경
            TaskbarItemInfo = taskbarInfo;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            taskbarInfo.ProgressState = TaskbarItemProgressState.Normal; // 작업 표시줄 상태 설정
            // 작업 표시줄 색상 설정
            taskbarInfo.ProgressValue = 0; // 0과 1 사이의 값으로 작업 표시줄의 색상을 변경합니다.
            taskbarInfo.ProgressState = TaskbarItemProgressState.Error; // 주황색 표시줄로 변경
            TaskbarItemInfo = taskbarInfo;

        }
    }
}
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Pomodoro.JsonManager;

namespace Pomodoro
{
    /// <summary>
    /// Setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Setting : Window
    {
        DataModel loadedData = null;

        string timer_hash = "#FFF1EEDC";
        string border_hash = "#FFB3C8CF";
        string second_hash = "#FFBED7DC";

        double scale = 0.5;
        double border = 10;
        double border_raidus = 20;

        Color selectedColor;
        String colorCode;
        private MainWindow mainWindow = null;
        public Setting(MainWindow parent)
        {
            InitializeComponent();
            mainWindow = parent;

            Loaded();
        }

        private void Loaded()
        {
            try
            {
                loadedData = DataModel.LoadFromJson("timer.json");
                Console.WriteLine("Data Loaded Successfully.");
                Console.WriteLine(loadedData.Color.Timer);

                mainWindow.timer_hash = loadedData.Color.Timer;
                mainWindow.border_hash = loadedData.Color.Border;
                mainWindow.second_hash = loadedData.Color.Middle;
                mainWindow.Resize(loadedData.Scale.Timer);
                mainWindow.Rect_Form.StrokeThickness = loadedData.Scale.Border;
                mainWindow.Rect_Form.RadiusX = loadedData.Scale.Radius;
                mainWindow.Rect_Form.RadiusY = loadedData.Scale.Radius;

                WorkHourBox.Text = loadedData.Time.Work.Hour.ToString();
                WorkMinuteBox.Text = loadedData.Time.Work.Minute.ToString();
                WorkSecondBox.Text = loadedData.Time.Work.Second.ToString();

                RestHourBox.Text = loadedData.Time.Rest.Hour.ToString();
                RestMinuteBox.Text = loadedData.Time.Rest.Minute.ToString();
                RestSecondBox.Text = loadedData.Time.Rest.Second.ToString();

                CntTextBox.Text = loadedData.Time.Loop.ToString();

                mainWindow.second = loadedData.Time.Work.Hour * 360 + loadedData.Time.Work.Minute * 60 + loadedData.Time.Work.Second;
                mainWindow.restSecond = loadedData.Time.Rest.Hour * 360 + loadedData.Time.Rest.Minute * 60 + loadedData.Time.Rest.Second;

                mainWindow.workCnt = loadedData.Time.Loop;

                Timer_Slider.Value = loadedData.Scale.Timer;
                Boarder_Slider.Value = loadedData.Scale.Border;
                Boarder_Radius_Slider_.Value = loadedData.Scale.Radius;
            }

            catch (FileNotFoundException ex)
            {
                MessageBox.Show("오류가 발생했습니다. 기본 값으로 초기화 합니다");
                loadedData.Time.Work.Hour = 0;
                loadedData.Time.Work.Minute = 25;
                loadedData.Time.Work.Second = 0;

                loadedData.Time.Rest.Hour = 0;
                loadedData.Time.Rest.Minute = 10;
                loadedData.Time.Rest.Second = 0;

                loadedData.Scale.Timer = 0.5;
                loadedData.Scale.Border = 10.0;
                loadedData.Scale.Radius = 10.0;

                loadedData.Color.Timer = timer_hash;
                loadedData.Color.Border = border_hash;
                loadedData.Color.Middle = second_hash; 
                
                loadedData.Time.Loop = 2;
                loadedData.SaveToJson("timer.json");

                this.Close();
                mainWindow.Close();
                Console.WriteLine(ex.Message);
            }

            Color color = (Color)ColorConverter.ConvertFromString(mainWindow.timer_hash);
            Timer_ColorPicker.SelectedColor = color;

            color = (Color)ColorConverter.ConvertFromString(mainWindow.border_hash);
            Border_ColorPicker.SelectedColor = color;

            color = (Color)ColorConverter.ConvertFromString(mainWindow.second_hash);
            Second_ColorPicker.SelectedColor = color;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Color color = (Color)ColorConverter.ConvertFromString(timer_hash);
            Timer_ColorPicker.SelectedColor = color;

            color = (Color)ColorConverter.ConvertFromString(border_hash);
            Border_ColorPicker.SelectedColor = color;

            color = (Color)ColorConverter.ConvertFromString(second_hash);
            Second_ColorPicker.SelectedColor = color;

            loadedData.Color.Timer = timer_hash;
            loadedData.Color.Border = border_hash;
            loadedData.Color.Middle = second_hash;

        }

        private void SizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Timer_Slider.Value = scale;
            this.Boarder_Slider.Value = border;
            this.Boarder_Radius_Slider_.Value = border_raidus;

            loadedData.Scale.Timer = scale;
            loadedData.Scale.Border = border;
            loadedData.Scale.Radius = border_raidus;

        }

        //스케일 조정
        private void Timer_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = e.NewValue;
            if(mainWindow == null)
            {
                return;
            }
            mainWindow.Resize(scale);
            loadedData.Scale.Timer = scale;
        }

        private void Boarder_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = e.NewValue;
            if (mainWindow == null)
            {
                return;
            }
            mainWindow.Rect_Form.StrokeThickness = value;
            loadedData.Scale.Border = value;

        }
        private void Boarder_Radius_Slider__ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = e.NewValue;
            if (mainWindow == null)
            {
                return;
            }
            mainWindow.Rect_Form.RadiusX = value;
            mainWindow.Rect_Form.RadiusY = value;
            loadedData.Scale.Radius = value;

        }



        //Color Change
        private void Border_ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (mainWindow == null)
            {
                return;
            }
            //선택된 색상을 가져옵니다.
            selectedColor = Border_ColorPicker.SelectedColor;

            colorCode = selectedColor.ToString();
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);
            SolidColorBrush brush = new SolidColorBrush(color);
            mainWindow.Rect_Form.Stroke = brush;
            loadedData.Color.Border = colorCode;

        }

        private void Timer_ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (mainWindow == null)
            {
                return;
            }
            //선택된 색상을 가져옵니다.
            selectedColor = Timer_ColorPicker.SelectedColor;

            colorCode = selectedColor.ToString();
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);
            SolidColorBrush brush = new SolidColorBrush(color);
            mainWindow.setTimerColor(brush);
            loadedData.Color.Timer = colorCode;

        }

        private void Second_ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {

            if (mainWindow == null)
            {
                return;
            }
            //선택된 색상을 가져옵니다.
            selectedColor = Second_ColorPicker.SelectedColor;

            colorCode = selectedColor.ToString();
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);
            SolidColorBrush brush = new SolidColorBrush(color);
            mainWindow.Middle_Stick.Fill = brush;
            loadedData.Color.Middle = colorCode;
        }



        private void TimeSave_Click(object sender, RoutedEventArgs e)
        {
            int workHour = WorkHourBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkHourBox.Text);
            int workMinute = WorkMinuteBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkMinuteBox.Text);
            int workSecond = WorkSecondBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkSecondBox.Text);

            int restHour = RestHourBox.Text.Trim() == "" ? 0 : Int32.Parse(RestHourBox.Text);
            int restMinute = RestMinuteBox.Text.Trim() == "" ? 0 : Int32.Parse(RestMinuteBox.Text);
            int restSecond = RestSecondBox.Text.Trim() == "" ? 0 : Int32.Parse(RestSecondBox.Text);

            int workCnt = CntTextBox.Text.Trim() == "" ? 0 : Int32.Parse(CntTextBox.Text);

            int workTime = (workHour * 60 * 60) + (workMinute * 60) + (workSecond);
            int restTIme = (restHour * 60 * 60) + (restMinute * 60) + (restSecond);

            loadedData.Time.Work.Hour = workHour;
            loadedData.Time.Work.Minute = workMinute;
            loadedData.Time.Work.Second = workSecond;
            loadedData.Time.Rest.Hour = restHour;
            loadedData.Time.Rest.Minute = restMinute;
            loadedData.Time.Rest.Second = restSecond;
            loadedData.Time.Loop = workCnt;

            mainWindow.second = workTime;
            mainWindow.restSecond = restTIme;
            mainWindow.workCnt = workCnt;

            mainWindow.setTimer();
        }

        private void checkText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;
            textBox.Text = text.Trim();
        }

        private void VerPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^\\d]+");
            e.Handled = regex.IsMatch(e.Text);
            TextBox textBox = (TextBox)sender;
            textBox.MaxLength = 2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loadedData.SaveToJson("timer.json");
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }
    }
}

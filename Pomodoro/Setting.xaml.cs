using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace Pomodoro
{
    /// <summary>
    /// Setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Setting : Window
    {
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
        }

        private void SizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Timer_Slider.Value = scale;
            this.Boarder_Slider.Value = border;
            this.Boarder_Radius_Slider_.Value = border_raidus;
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
        }

        private void Boarder_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = e.NewValue;
            if (mainWindow == null)
            {
                return;
            }
            mainWindow.Rect_Form.StrokeThickness = value;

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
        }



        private void TimeSave_Click(object sender, RoutedEventArgs e)
        {
            int workHour = WorkHourBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkHourBox.Text);
            int workMinute = WorkMinuteBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkMinuteBox.Text);
            int workSecond = WorkSecondBox.Text.Trim() == "" ? 0 : Int32.Parse(WorkSecondBox.Text);

            int restHour = RestHourBox.Text.Trim() == "" ? 0 : Int32.Parse(RestHourBox.Text);
            int restMinute = RestMinuteBox.Text.Trim() == "" ? 0 : Int32.Parse(RestMinuteBox.Text);
            int restSecond = RestSecondBox.Text.Trim() == "" ? 0 : Int32.Parse(RestSecondBox.Text);

            int workTime = (workHour * 60 * 60) + (workMinute * 60) + (workSecond);
            int restTIme = (restHour * 60 * 60) + (restMinute * 60) + (restSecond);

            Console.WriteLine(workTime + " " + restTIme);

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
    }
}

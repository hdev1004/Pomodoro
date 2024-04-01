using MaterialDesignThemes.Wpf;
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

namespace Pomodoro
{
    /// <summary>
    /// Setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Setting : Window
    {
        Color selectedColor;
        String colorCode;
        private MainWindow mainWindow;
        public Setting(MainWindow parent)
        {
            InitializeComponent();
            mainWindow = parent;
        }

        private void Pix_ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            // 선택된 색상을 가져옵니다.
            selectedColor = Pix_ColorPicker.SelectedColor;

            colorCode = selectedColor.ToString();
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);
            SolidColorBrush brush = new SolidColorBrush(color);
            mainWindow.Rect_Form.Stroke = brush;
            
            Console.WriteLine(colorCode);
        }
    }
}

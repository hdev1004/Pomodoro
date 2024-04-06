using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;


namespace Pomodoro
{
    internal class JsonManager
    {
        public class DataModel
        {
            public TimeData Time { get; set; }
            public ScaleData Scale { get; set; }
            public ColorData Color { get; set; }

            // JSON 파일에서 데이터를 불러오는 메서드
            public static DataModel LoadFromJson(string filePath)
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<DataModel>(jsonData);
                }
                else
                {
                    throw new FileNotFoundException("File not found.", filePath);
                }
            }

            // 데이터를 JSON 파일로 저장하는 메서드
            public void SaveToJson(string filePath)
            {
                string jsonData = JsonConvert.SerializeObject(this);
                File.WriteAllText(filePath, jsonData);
            }
        }

        public class TimeData
        {
            public TimeInfo Work { get; set; }
            public TimeInfo Rest { get; set; }
            public int Loop { get; set; }
        }

        public class ScaleData
        {
            public double Timer { get; set; }
            public double Border { get; set; }
            public double Radius { get; set; }
        }

        public class ColorData
        {
            public string Border { get; set; }
            public string Timer { get; set; }
            public string Middle { get; set; }
        }

        public class TimeInfo
        {
            public int Hour { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }
        }


        public JsonManager()
        {

            // 변수를 JSON 파일로 저장하는 메서드
            void SaveDataToJson(DataModel data, string fileName)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                string jsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, jsonData);
            }

            // JSON 파일에서 변수를 불러오는 메서드
            DataModel LoadDataFromJson(string fileName)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<DataModel>(jsonData);
                }
                else
                {
                    MessageBox.Show("File not found.");
                    return null;
                }
            }
        }

    }

}

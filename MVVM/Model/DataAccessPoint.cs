using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
//using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Markup;
namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    internal class DataAccessPoint
    {
        // Setup Training Data
        private const string TRAINING_DATA_PATH = "..\\..\\DataSource\\training_data.ser";
        public List<Training_Data> _data { get; set; } = new List<Training_Data>();


        public DataAccessPoint()
        {
            try
            {
                _data = JsonConvert.DeserializeObject<List<Training_Data>>(File.ReadAllText(TRAINING_DATA_PATH));
            }
            catch (FileNotFoundException file_not_found)
            {
                Console.WriteLine(file_not_found);
                _data = new List<Training_Data>();
            }
        }


        // save data
        public void SerializeObject(string filePath, object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, json);
        }


        public void SaveAllTrainingData(string save_training_data_path = TRAINING_DATA_PATH)
        {
            if (_data != null || _data.Count > 0)
            {
                SerializeObject(save_training_data_path, _data);
            }
        }


        // get data
        public Training_Data DeserializeObject_Training_Data(string filePath)
        {
            return JsonConvert.DeserializeObject<Training_Data>(File.ReadAllText(filePath));
        }


        // create data
        public static Training_Data Create_Training_Data(string target_value, List<double> rowAverages, List<double> columnAverages, int totalImgUsedToTrain = 1)
        {
            Training_Data new_training_data = new Training_Data()
            {
                Value = target_value,
                TotalImagesUsedToTrain = totalImgUsedToTrain,
                RowAverages = rowAverages,
                ColumnAverages = columnAverages
            };

            return new_training_data;
        }



        // update data
        public void UpdateTrainingDataByValue(string value, List<double> newRowAverages, List<double> newColumnAverages)
        {
            // Get target value
            int target_data_index = -1;
            for (int index = 0; index < _data.Count; index++)
            {
                if (_data[index].Value == value)
                {
                    target_data_index = index;
                    break;
                }
            }

            // if index is -1, no targets data was found. Create new data for specified value
            if (target_data_index == -1)
            {
                // Add functionality so buttons are added to the UI
                _data.Add(Create_Training_Data(value, newRowAverages, newColumnAverages));
                SaveAllTrainingData();
                return;
            }

            Training_Data target_data = _data[target_data_index];

            // update training data averages with new input data
            List<double> adjustedRowAverages = target_data.RowAverages;
            List<double> adjustedColumnAverages = target_data.ColumnAverages;

            for (int data_index = 0; data_index < adjustedRowAverages.Count; data_index++)
            {
                adjustedRowAverages[data_index] *= target_data.TotalImagesUsedToTrain;
                adjustedRowAverages[data_index] += newRowAverages[data_index];
                adjustedRowAverages[data_index] /= (target_data.TotalImagesUsedToTrain + 1);

                adjustedColumnAverages[data_index] *= target_data.TotalImagesUsedToTrain;
                adjustedColumnAverages[data_index] += newColumnAverages[data_index];
                adjustedColumnAverages[data_index] /= target_data.TotalImagesUsedToTrain + 1;
            }

            target_data.TotalImagesUsedToTrain += 1;
            target_data.RowAverages = adjustedRowAverages;
            target_data.ColumnAverages = adjustedColumnAverages;

            _data[target_data_index] = target_data;
            SaveAllTrainingData();
        }

        public int GetDataIndexByValue(string value)
        {
            for (int index = 0; index < _data.Count; index++)
            {
                // get value from training_data that matches button content
                if (_data[index].Value == value) { return index; }
            }
            return -1;
        }

        public string CreateTrainingDataReport(string filePath = "..\\..\\DataSource\\training_data_report.txt")
        {
            string report = "General Information";
            report += "\n\n Total Values Trained for OCR:\t" + _data.Count;

            // Run accuracy

            foreach (var trained_data in _data)
            {
                report += "\n\nValue:\t\t\t\t" + ((trained_data.Value.Length == 0) ? "\"\"" : trained_data.Value);
                report += "\nCount of images used to train:\t" + trained_data.TotalImagesUsedToTrain;
            }

            return report;

        }


        // CLR Objects

        public class Training_Data
        {
            public string Value { get; set; }
            public int TotalImagesUsedToTrain { get; set; } = 0;
            public List<double> RowAverages { get; set; }
            public List<double> ColumnAverages { get; set; }
        }
    }
}
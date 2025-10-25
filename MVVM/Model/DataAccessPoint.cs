using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    public class DataAccessPoint
    {
        // Setup Training Data
        private const string DIRECTORY_PATH = "..\\..\\DataSource\\";
        private const string FILE_NAME = "training_data.ser";
        private const string TRAINING_DATA_PATH = DIRECTORY_PATH + FILE_NAME;
        public List<Training_Data> _data { get; set; } = new List<Training_Data>();

        public DataAccessPoint()
        {
            if (!Directory.Exists(DIRECTORY_PATH))
                Directory.CreateDirectory(DIRECTORY_PATH);

            if (!File.Exists(TRAINING_DATA_PATH))
                File.Create(TRAINING_DATA_PATH);

            // This will crash the first time if file was just created.
            // Just launch the app again and it'll work fine
            var fileData = File.ReadAllText(TRAINING_DATA_PATH);

            if (string.IsNullOrEmpty(fileData))
                fileData = "[]";
            
            _data = JsonConvert.DeserializeObject<List<Training_Data>>(fileData) ?? new List<Training_Data>();
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
            // Get target index so we can set the element later
            int target_data_index = GetDataIndexByValue(value);

            // Create new training value if target was not found.
            if (target_data_index == -1)
            {
                // Update UI with buttons
                _data.Add(Create_Training_Data(value, newRowAverages, newColumnAverages));
                SaveAllTrainingData();
                return;
            }

            Training_Data target_data = _data[target_data_index];

            // update training data averages with new input data
            List<double> adjustedRowAverages = target_data.RowAverages.ToList();
            List<double> adjustedColumnAverages = target_data.ColumnAverages.ToList();

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

        public int GetDataIndexByValue(string value) => _data.FindIndex(a => a.Value == value);

        // Generates string that displays information about the currently trained data
        public string CreateTrainingDataReport(string fileName = "training_data_report.txt")
        {
            var filePath = DIRECTORY_PATH + fileName;

            if (!File.Exists(filePath)) 
                File.Create(filePath);

            // Header
            string report = "General Information";
            report += "\n\n Total Values Trained for OCR:\t" + _data.Count;

            // Display trained value data
            foreach (var trained_data in _data)
            {
                report += "\n\nValue:\t\t\t\t" + ((trained_data.Value.Length == 0) ? "\"\"" : trained_data.Value);
                report += "\nCount of images used to train:\t" + trained_data.TotalImagesUsedToTrain;
            }

            File.WriteAllText(filePath, report);

            return report;
        }
    }
}
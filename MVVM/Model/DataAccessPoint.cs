using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    internal class DataAccessPoint
    {
        // Setup Training Data
        private const string TRAINING_DATA_PATH = "..\\..\\DataSource\\training_data.ser";

        public DataAccessPoint()
        {
            try
            {
                _data = JsonSerializer.Deserialize<List<Training_Data>>(File.ReadAllText(TRAINING_DATA_PATH));
            }
            catch (FileNotFoundException file_not_found)
            {
                Console.WriteLine(file_not_found);
                _data = new List<Training_Data>();
            }
            Console.WriteLine("AVERY DEBUG: " + _data.ToString());
        }



        // save data
        public void SerializeObject(string filePath, object obj)
        {
            var json = JsonSerializer.Serialize(obj);
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
            return JsonSerializer.Deserialize<Training_Data>(File.ReadAllText(filePath));
        }

        //public All_Training_Data DeserializeObject_All_Data(string filePath)
        //{
        //    return JsonSerializer.Deserialize<All_Training_Data>(File.ReadAllText(filePath));
        //}



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

        // CLR Objects

        public List<Training_Data> _data { get; set; } = new List<Training_Data>();


        public class Training_Data
        {
            public string Value { get; set; }
            public int TotalImagesUsedToTrain { get; set; } = 0;
            public List<double> RowAverages { get; set; }
            public List<double> ColumnAverages { get; set; }
        }

    }
}



/*
using System.Text.Json;
using System.Text.Json.Serialization;

List<data> _data = new List<data>();
_data.Add(new data()
{
    Id = 1,
    SSN = 2,
    Message = "A Message"
});

string json = JsonSerializer.Serialize(_data);
File.WriteAllText(@"D:\path.json", json);
*/


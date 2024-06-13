using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static Custom_Optical_Character_Recognition_System.MVVM.Model.DataAccessPoint;

namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    internal class DataAlgorithm
    {

        /// <summary>
        /// Returns tuple of list rowsAverage and columnsAverage of picture at specified path.
        /// SOON TO BE USELESS
        /// </summary>
        /// <returns></returns>
        public (List<double>, List<double>) GetImageData(Bitmap bitmap)
        {
            List<double> columnsAverage = new List<double>();
            List<double> rowsAverage = new List<double>();
            for (int col = 0; col < bitmap.Width; col++)
            {
                double coloredColumnPixelCount = 0;
                double coloredRowPixelCount = 0;
                for (int row = 0; row < bitmap.Height; row++)
                {
                    // Setup Row sums
                    coloredRowPixelCount += bitmap.GetPixel(col, row).GetBrightness();
                    // Setup Column sums
                    coloredColumnPixelCount += bitmap.GetPixel(row, col).GetBrightness();
                }

                // Add Averages to data lists
                rowsAverage.Add(coloredRowPixelCount / bitmap.Width);
                columnsAverage.Add(coloredColumnPixelCount / bitmap.Height);
            }

            return (rowsAverage, columnsAverage);
        }

        //----
        public string RecognizeValueFromData(List<double> row_input_data, List<double> column_input_data)
        {
            // Get training data
            DataAccessPoint DAO = new DataAccessPoint();
            List<Training_Data> all_training_data = DAO._data;

            // Used to point to current likliest value
            double lowest_difference = row_input_data.Count();

            // Used 
            string recognized_value = "";
            foreach (Training_Data training_data in all_training_data)
            {
                double difference_rate = (CompareInputDataToTrainingData(training_data.RowAverages, row_input_data) + CompareInputDataToTrainingData(training_data.ColumnAverages, column_input_data)) / 2;
                if (difference_rate < lowest_difference)
                {
                    lowest_difference = difference_rate;
                    recognized_value = "Value Identified: " + training_data.Value + "   |   Image training Count: " + training_data.TotalImagesUsedToTrain;
                }
            }
            return recognized_value;
        }


        /// <summary>
        /// When subtracting the elements from the training data to the elements of the input data, you end up with a differenceRate.
        /// This is used to see how different the input data is to the training data. As differenceRate approaches 0, it'll representitively
        /// show how likely the current training data is to match with the input data.
        /// </summary>
        /// <param name="training_data_path"></param>
        /// <param name="input_data_path"></param>
        public double CompareInputDataToTrainingData(List<double> training_data, List<double> input_data)
        {
            double differenceRate = 0;

            for (int index = 0; index < training_data.Count; index++)
            {
                // difference between training data and input data
                var data_difference = training_data[index] - input_data[index];

                // if differenceRate is negative, force it into positive
                differenceRate += (data_difference < 0 ? data_difference * -1 : data_difference);
            }

            // Return average difference
            return differenceRate / training_data.Count;
        }
    }
}

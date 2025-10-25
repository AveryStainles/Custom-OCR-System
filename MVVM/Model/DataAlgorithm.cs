using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    internal class DataAlgorithm
    {

        // Returns tuple of list rowsAverage and columnsAverage of picture at specified path.
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

        public string RecognizeValueFromData(List<double> rowInputData, List<double> columnInputData)
        {
            // Get training data
            DataAccessPoint DAO = new DataAccessPoint();
            List<Training_Data> allTrainingData = DAO._data;

            // The lower the number, the liklier the value is the target
            double lowestDifference = rowInputData.Count();
            string targetValue = "";

            foreach (Training_Data training_data in allTrainingData)
            {
                var compareRowData = CompareInputDataToTrainingData(training_data.RowAverages.ToList(), rowInputData);
                var compareColumnData = CompareInputDataToTrainingData(training_data.ColumnAverages.ToList(), columnInputData);
                double differenceRate = (compareRowData + compareColumnData) / 2;

                if (differenceRate < lowestDifference)
                {
                    lowestDifference = differenceRate;
                    targetValue = training_data.Value;
                }
            }
            return targetValue;
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

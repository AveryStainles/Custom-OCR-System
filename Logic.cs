using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Custom_Optical_Character_Recognition_System
{
    internal class Logic
    {
        private Read_Write_HelperClass helper = new Read_Write_HelperClass();

        public void GenerateTrainingDataFromTrainingImages(string data_path)
        {
            Algorithm algorithm = new Algorithm();
            for (int folder_num = 0; folder_num <= 9; folder_num++)
            {
                List<double> sum_of_column_averages = new List<double>();
                List<double> sum_of_row_averages = new List<double>();
                string folder_path = data_path + folder_num;
                int total_img_count = Directory.GetFiles($"{folder_path}\\Training_Images").Count();
                for (int image_file_count = 0; image_file_count < total_img_count; image_file_count++)
                {
                    //setup paths
                    string image_path = $"{folder_path}\\Training_Images\\Hand_Drawn_ ({image_file_count}).png";
                    string save_location_path = $"{folder_path}\\Training_Data\\";

                    //setup data
                    //Item1 = List<double> rows, Item2 = List<double> columns
                    (List<double>, List<double>) row_column_averages = GetImageData(image_path);

                    helper.WriteToFile($"{save_location_path}row_averages{image_file_count}.csv", String.Join(",", row_column_averages.Item1));
                    helper.WriteToFile($"{save_location_path}column_averages{image_file_count}.csv", String.Join(",", row_column_averages.Item2));

                    // if this is first loop, set lists equal to image data. Otherwise itll add the elements to its respective indecies.
                    if (sum_of_row_averages.Count == 0 && sum_of_column_averages.Count == 0)
                    {
                        sum_of_row_averages = row_column_averages.Item1;
                        sum_of_column_averages = row_column_averages.Item2;
                        continue;
                    }

                    // add row and column values to respective lists
                    for (int index = 0; index < row_column_averages.Item1.Count; index++)
                    {
                        sum_of_row_averages[index] += row_column_averages.Item1[index];
                        sum_of_column_averages[index] += row_column_averages.Item2[index];
                    }
                }
                // Sets the overall average of the training timage's data
                for (int index = 0; index < sum_of_row_averages.Count; index++)
                {
                    sum_of_row_averages[index] = Math.Round(sum_of_row_averages[index] / total_img_count, 5);
                    sum_of_column_averages[index] = Math.Round(sum_of_column_averages[index] / total_img_count, 5);
                }

                sum_of_row_averages = LineUpData(sum_of_row_averages);
                sum_of_column_averages = LineUpData(sum_of_column_averages);

                helper.WriteToFile($"{folder_path}\\Averages\\row.csv", String.Join(",", sum_of_row_averages));
                helper.WriteToFile($"{folder_path}\\Averages\\column.csv", String.Join(",", sum_of_column_averages));
            }
        }

        /// <summary>
        /// Returns tuple of list rowsAverage and columnsAverage of picture at specified path.
        /// SOON TO BE USELESS
        /// </summary>
        /// <param name="img_folder_path"></param>
        /// <param name="img_file_name"></param>
        /// <returns></returns>
        public (List<double>, List<double>) GetImageData(string imageFilePath)
        {
            Bitmap image = new Bitmap(imageFilePath + "");
            int IMG_WIDTH = image.Width;
            int IMG_HEIGHT = image.Height;
            List<double> columnsAverage = new List<double>();
            List<double> rowsAverage = new List<double>();
            for (int col = 0; col < IMG_WIDTH; col++)
            {
                double coloredColumnPixelCount = 0;
                double coloredRowPixelCount = 0;
                for (int row = 0; row < IMG_HEIGHT; row++)
                {
                    // Setup Row sums
                    coloredColumnPixelCount += image.GetPixel(row, col).GetBrightness(); ;
                    // Setup Column sums
                    coloredRowPixelCount += image.GetPixel(col, row).GetBrightness(); ;
                }

                // Add Averages to data lists
                rowsAverage.Add(coloredRowPixelCount / IMG_WIDTH);
                columnsAverage.Add(coloredColumnPixelCount / IMG_HEIGHT);
            }

            // setup data to have the highest pixel dansity in the bottom right corner
            rowsAverage = LineUpData(rowsAverage);
            columnsAverage = LineUpData(columnsAverage);
            return (rowsAverage, columnsAverage);
        }

        public List<double> LineUpData(List<double> data)
        {
            // setup data to have the highest pixel dansity in the bottom right corner
            Algorithm algorithm = new Algorithm();
            List<double> temp_data = data;
            while (data[data.Count - 1] < data[data.Count - 2])
            {
                algorithm.CycleValues(temp_data);
            }
            data = temp_data;

            return data;
        }


        public string RenderImage(string imageFilePath)
        {
            Bitmap image = new Bitmap(imageFilePath);
            int IMG_WIDTH = image.Width;
            int IMG_HEIGHT = image.Height;
            string image_ui_render = "";
            float pixelColor;
            for (int col = 0; col < IMG_HEIGHT; col++)
            {
                for (int row = 0; row < IMG_WIDTH; row++)
                {
                    pixelColor = image.GetPixel(row, col).GetBrightness();
                    image_ui_render += (pixelColor == 0) ? $"{pixelColor} " : ". ";
                }
                image_ui_render += "\n";
            }

            return image_ui_render;
        }

        public string RenderImageFromData(List<double> row_data, List<double> column_data, double heat_sensitivity_rate = 1.3)
        {
            string display_data = "";
            for (int row = 0; row < row_data.Count; row++)
            {
                for (int column = 0; column < column_data.Count; column++)
                {
                    display_data += ((column_data[column] + row_data[row]) > heat_sensitivity_rate) ? ". " : "0 ";
                }
                display_data += "\n";
            }
            return display_data;
        }

        public class Algorithm
        {
            private Read_Write_HelperClass helperClass = new Read_Write_HelperClass();

            public Algorithm() { }


            // IDEA: Restrict canvas sizes to multiples of 64
            public List<double> ScaleDataDown(List<double> data)
            {
                int normalized_image_scale = 14;
                List<double> scaled_data = new List<double>() { 0 };

                if (data.Count < normalized_image_scale)
                {
                    return null;
                }

                double scale_rate_cursor = data.Count() / normalized_image_scale;
                double default_cursor_rate = scale_rate_cursor;
                int scaled_data_index = 0;
                for (int large_data_index = 0; large_data_index < data.Count(); large_data_index++)
                {
                    // When the cursor is above 0, just collect the sum of the value at the specified index
                    // Algorithm increments forloop index so it makes sure to prevent breaking 
                    if (scale_rate_cursor > 1 || (large_data_index + 1) > data.Count())
                    {
                        scaled_data[scaled_data_index] += data[large_data_index] / default_cursor_rate; // ERROR HERE | OUT OF BOUND EXCEPTION
                        scale_rate_cursor -= 1;
                        continue;
                    }
                    // When the cursor is below 1 it might need a fraction of the value. By multiplying the double (e.g. '0.3')
                    // with the data, you get the respective fraction of that value. Then by inversing it you can give the
                    // remaining representative cell value like so f(0.3) -> { 0.3 - 1 == -0.7 | -0.7 * -1 == 0.7)
                    scaled_data[scaled_data_index] += (scale_rate_cursor * data[large_data_index]) / default_cursor_rate;

                    scale_rate_cursor -= 1;
                    scale_rate_cursor *= -1;
                    scaled_data_index++;

                    scaled_data.Add((scale_rate_cursor * data[large_data_index]) / default_cursor_rate);
                    scale_rate_cursor = default_cursor_rate - scale_rate_cursor;
                }

                return scaled_data;
            }



            /// <summary>
            /// Removes the last item of the and inserts it at the front
            /// </summary>
            /// <param name="list"></param>
            public void CycleValues(List<double> list)
            {
                var last_element = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                list.Insert(0, last_element);
            }

            /// <summary>
            /// When subtracting the elements from the training data to the elements of the input data, you end up with a differenceRate.
            /// This is used to see how different the input data is to the training data. As differenceRate approaches 0, it'll representitively
            /// show how likely the current training data is to match with the input data.
            /// </summary>
            /// <param name="training_data_path"></param>
            /// <param name="input_data_path"></param>
            public double CompareInputDataToTrainingData(string training_data_path, string input_data_path)
            {
                List<double> training_data = helperClass.GetListFromDataPath(training_data_path);
                List<double> user_data = helperClass.GetListFromDataPath(input_data_path);
                double differenceRate = 0;

                for (int index = 0; index < training_data.Count; index++)
                {
                    // difference between training data and input data
                    var data_difference = training_data[index] - user_data[index];

                    // if differenceRate is negative, force it into positive
                    differenceRate += (data_difference < 0 ? data_difference * -1 : data_difference);
                }

                // Return average difference
                return differenceRate / training_data.Count;
            }
        }
    }
}

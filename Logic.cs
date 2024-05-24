namespace AverySecretProject
{
    internal class Logic
    {
        private Read_Write_HelperClass helper = new();

        public void SetImageData(string current_num_folder_path, int total_num_photo_to_convert, string save_destination_file_path)
        {
            List<double> current_image_column_agerages;
            List<double> current_image_row_agerages;
            for (int img_count = 1; img_count <= total_num_photo_to_convert; img_count++)
            {
                string current_folder_num = current_num_folder_path[current_num_folder_path.Length - 1] + "";
                var hold_val = GetImageData(current_num_folder_path + "\\" + "Hand_Drawn_" + current_folder_num + " " + img_count + ".png");
                current_image_column_agerages = hold_val.Item1;
                current_image_row_agerages = hold_val.Item2;

                helper.WriteToFile($"{save_destination_file_path}\\columns_data_{img_count}.csv", String.Join(",", current_image_column_agerages));
                helper.WriteToFile($"{save_destination_file_path}\\rows_data_{img_count}.csv", String.Join(",", current_image_row_agerages));
            }
        }


        public void LoadAverageColumnRowDataToNewCSV(string filePath, string file_name, string save_destination, int data_for_num)
        {
            List<double> default_list = helper.ReadFromFile($"{filePath}{file_name}1.csv").Split(',').Select(num => Convert.ToDouble(num)).ToList();
            List<double>? current_list = null;
            for (int i = 1; i <= 50; i++)
            {
                current_list = helper.ReadFromFile($"{filePath}{file_name}{i}.csv").Split(',').Select(num => Convert.ToDouble(num)).ToList();
                for (int list_index = 0; list_index < default_list.Count; list_index++)
                {
                    default_list[list_index] = Math.Round((default_list[list_index] + current_list[list_index]) / 2, 3);
                }
            }

            helper.WriteToFile(save_destination + $"{data_for_num}.csv", String.Join(",", default_list));
        }


        public void GenerateTrainingDataFromTrainingImages(string data_path)
        {
            for (int folder_num = 0; folder_num <= 9; folder_num++)
            {
                List<double>? sum_of_column_averages = new();
                List<double>? sum_of_row_averages = new();
                string folder_path = data_path + folder_num;
                int total_img_count = 50;
                for (int image_file_count = 0; image_file_count <= total_img_count-1; image_file_count++)
                {
                    //setup paths
                    string image_path = $"{folder_path}\\Training_Images\\Hand_Drawn_ ({image_file_count}).png";
                    string save_location_path = $"{folder_path}\\Training_Data\\";

                    //setup data           row_column_averages = (Item1 = List<double> rows, Item2 = List<double> columns)
                    var row_column_averages = GetImageData(image_path);

                    // if this is first loop, set lists equal to image data. Otherwise itll append the doubles later.
                    if (sum_of_row_averages.Count == 0 && sum_of_column_averages.Count == 0)
                    {
                        sum_of_row_averages = row_column_averages.Item1;
                        sum_of_column_averages = row_column_averages.Item2;
                        continue;
                    }

                    helper.WriteToFile($"{save_location_path}row_averages{image_file_count}.csv", String.Join(",", row_column_averages.Item1));
                    helper.WriteToFile($"{save_location_path}column_averages{image_file_count}.csv", String.Join(",", row_column_averages.Item2));

     // TODO CYCLE THE LISTS SO THE TRAINING DATA IS LINED UP WITH ITSELF BEFORE TO THE SUM LISTS
                    // add row and column values to respective lists
                    for (int index = 0; index < row_column_averages.Item1.Count; index++)
                    { 
                        sum_of_row_averages[index] += row_column_averages.Item1[index];
                        sum_of_column_averages[index] += row_column_averages.Item1[index];
                    }
                }
                // Sets the overall average of the training timage's data
                for (int index = 0; index < sum_of_row_averages.Count; index++)
                {
                    sum_of_row_averages[index] = Math.Round(sum_of_row_averages[index]/total_img_count, 5);
                    sum_of_column_averages[index] = Math.Round(sum_of_column_averages[index]/total_img_count, 5);
                }

                helper.WriteToFile($"{folder_path}\\Averages\\row.csv", String.Join(",", sum_of_row_averages));
                helper.WriteToFile($"{folder_path}\\Averages\\column.csv", String.Join(",", sum_of_column_averages));
            }
        }


        /// <summary>
        /// Returns tuple of list rowsAverage and columnsAverage of picture at specified path.
        /// </summary>
        /// <param name="img_folder_path"></param>
        /// <param name="img_file_name"></param>
        /// <returns></returns>
        public (List<double>, List<double>) GetImageData(string imageFilePath)
        {
            Bitmap image = new Bitmap(imageFilePath + "");
            int IMG_WIDTH = image.Width;
            int IMG_HEIGHT = image.Height;

            // Render image as greyscale values
            // White Pixel == 1   |   Black Pixel == 0
            List<double> columnsAverage = new();
            List<double> rowsAverage = new();
            float coloredColumnPixelCount = 0;
            float coloredRowPixelCount = 0;
            float pixelColor;
            for (int col = 0; col < IMG_HEIGHT; col++)
            {
                for (int row = 0; row < IMG_WIDTH; row++)
                {
                    // Setup Row averages
                    pixelColor = image.GetPixel(row, col).GetBrightness();
                    if (pixelColor == 0) { coloredColumnPixelCount++; }

                    // Setup Column averages
                    pixelColor = image.GetPixel(col, row).GetBrightness();
                    if (pixelColor == 0) { coloredRowPixelCount++; }
                }

                // Add Averages to data arrays rounded hundredth (0.00)
                rowsAverage.Add((coloredColumnPixelCount > 0) ? Math.Floor((coloredColumnPixelCount / 3) * 100) / 100 : 0);
                columnsAverage.Add((coloredRowPixelCount > 0) ? Math.Floor((coloredRowPixelCount / 3) * 100) / 100 : 0);

                coloredColumnPixelCount = 0;
                coloredRowPixelCount = 0;
            }

            return (rowsAverage, columnsAverage);
        }


        public string RenderImage(string imageFilePath)
        {
            Bitmap image = new(imageFilePath);
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

        public class Algorithm
        {
            private Read_Write_HelperClass helperClass = new();

            public Algorithm() { }

            /// <summary>
            /// Gets the difference rate from specified files
            /// </summary>
            public double CompareInputDataToTrainingData(string training_data_path, string input_data_path)
            {
                // Compare Difference Rate
                List<double> training_data_list = helperClass.ReadFromFile(training_data_path).Split(',').Select(num => Convert.ToDouble(num)).ToList();
                List<double> user_data_list = helperClass.ReadFromFile(input_data_path).Split(',').Select(num => Convert.ToDouble(num)).ToList();
                double rate = GetDifferenceRates(training_data_list, user_data_list);
                return rate;
            }

            /// <summary>
            /// Puts the last element of the array to the front of it. This lines up the png to the bottom right. Caution, this modifies the original array.
            /// </summary>
            /// <param name="list"></param>
            private void CycleValues(List<double> list)
            {
                //AVERY TEST
                if (list[list.Count - 1] != 0)
                {
                    return;
                }

                var last_element = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                list.Insert(0, last_element);
            }

            /// <summary>
            ///     Returns the average of the total rates
            /// </summary>
            /// <returns></returns>
            private double GetDifferenceRates(List<double> t_data, List<double> in_data)
            {
                double differenceRate = 100; // 100% different
                List<double> all_img_difference_rate = new();

                for (int cycle_index = 0; cycle_index < in_data.Count; cycle_index++)
                {
                    double img_difference_rate = GetComparedImagesRate(t_data, in_data);
                    all_img_difference_rate.Add(img_difference_rate);
                    CycleValues(in_data);

                    if (img_difference_rate < differenceRate) { differenceRate = img_difference_rate; }
                }


                return differenceRate;
                //return all_img_difference_rate;
            }

            private double GetComparedImagesRate(List<double> t_data, List<double> in_data)
            {
                double differenceRate = 0;

                // returns a cumulative difference between each values 
                // If the difference is negative, it is turned positive to avoid skewing data
                for (int index = 0; index < t_data.Count; index++)
                {

                    //AVERY TEST
                    if (in_data[index] == 0) { continue; }
                    var hold_value = t_data[index] - in_data[index];
                    differenceRate += (hold_value < 0 ? hold_value * -1 : hold_value);

                }

                var average_difference_rate = differenceRate / t_data.Count;
                return average_difference_rate;
            }

        }
    }
}

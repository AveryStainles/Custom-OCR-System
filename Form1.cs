namespace AverySecretProject
{
    public partial class Form1 : Form
    {

        private Read_Write_HelperClass helperClass = new();
        private Logic logic = new();
        private Logic.Algorithm algorithm = new();
        public Form1()
        {
            InitializeComponent();
            lbl_DisplayText.Left = 5;


            // Setup Paths
            string data_file_path = "C:\\Users\\fox-r\\source\\repos\\AverySecretProject\\Data\\";
            string input_image_file_path = data_file_path + "ImageInput.png";
            string training_data_path = data_file_path + "0\\Averages\\";

            //Demo_RenderImageFromTestData(training_data_path);
            //Demo_EvaluateInput(input_image_file_path, data_file_path);
            //ResetTrainingData(data_file_path);
            //RunAccuracyTest(data_file_path);
        }


        public void RenderInputImage(string img_file_path) { lbl_DisplayText.Text = logic.RenderImage(img_file_path); }

        public void SaveInputData(string save_location_path, string data) { helperClass.WriteToFile(save_location_path, data); }

        /// <summary>
        /// A quick demo to help render the overal heatmap of the data
        /// </summary>
        public void Demo_RenderImageFromTestData(string training_data_path)
        {
            // Setup Data
            List<double> row_data = helperClass.GetListFromDataPath(training_data_path + "row.csv");
            List<double> column_data = helperClass.GetListFromDataPath(training_data_path + "column.csv");

            // Render Image From Training Data
            double heat_sensitivity_rate = 1.3;
            string test = logic.RenderImageFromData(row_data, column_data, heat_sensitivity_rate);
            lbl_DisplayText.Text = test;

            // Load Data About The Render
            lblRowData.Text += $"Row Average: {Math.Round(row_data.Average(), 3)}\nCol Average: {Math.Round(column_data.Average(), 3)}" + "\n\n";
            for (int index = 0; index < row_data.Count; index++)
            {
                lblRowData.Text += ("Row: " + Math.Round(row_data[index], 3)).PadRight(20) + ("Col:" + Math.Round(column_data[index], 3) + "\n");
            }
        }

        /// <summary>
        /// Evaluates input data compared to training data to display the value the system recognized from the input.
        /// </summary>
        /// <param name="input_image_file_path">Full path of the .png file to analyze</param>
        /// <param name="data_file_path">>Path to the Data folder of the project as a string</param>
        public void Demo_EvaluateInput(string input_image_file_path, string data_file_path)
        {
            // Setup data
                // Item 1: row data     Item 2: column data
            (List<double>, List<double>) data = logic.GetImageData(input_image_file_path);
            SaveInputData(data_file_path + "input_rows.csv", String.Join(",", data.Item1));
            SaveInputData(data_file_path + "input_columns.csv", String.Join(",", data.Item2));
            (double, int) lowest_difference_rate = GetInputValFromAlgorithm(data_file_path);

            // Display data and user input image
            RenderInputImage(input_image_file_path);
            lblRowData.Text = "Val | Difference Rate:";
                // Item2 = number it thinks the user wrote      |       Item 1 = difference rate
            lblRowData.Text += "\n   " + lowest_difference_rate.Item2 + " | " + Math.Round(lowest_difference_rate.Item1, 3);
        }


        /// <summary>
        /// Performs 10 tests with different inputs on each number. (as of 2024-05-25 this is 100 inputs being tested
        /// </summary>
        /// <param name="data_file_path">Path to the Data folder of the project as a string</param>
        /// <returns></returns>
        public void RunAccuracyTest(string data_file_path)
        {
            string result = "";
            int total_score = 0;
            for (int folder_num = 0; folder_num <= 9; folder_num++)
            {
                int score = 0;
                string folder_path = data_file_path + folder_num + "\\";
                for (int file_num = 0; file_num <= 9; file_num++)
                {
                    string image_path = folder_path + $"Test_Input_Data\\ImageInput{file_num}.png";
                    var data = logic.GetImageData(image_path);

                    SaveInputData(data_file_path + "input_rows.csv", String.Join(",", data.Item1));
                    SaveInputData(data_file_path + "input_columns.csv", String.Join(",", data.Item2));

                    var lowest_difference_rate = GetInputValFromAlgorithm(data_file_path);
                    RenderInputImage(image_path);

                    if (lowest_difference_rate.Item2 == folder_num) { score++; }
                }
                result += "\n" + folder_num + " | " + score + "/10";
                total_score += score;
            }
            lblRowData.Text = $"Total Score: {total_score}%" + result;
        }

        /// <summary>
        /// returns a tuple with the data corresponding to the difference rate and the value the algorithm recognized.
        /// <para>Item 1: (double) Difference Rate</para>
        /// <para>Item 2: (int) Value the algorithm recognized</para>
        /// </summary>
        /// <param name="data_file_path"> Path to the Data folder of the project as a string</param>
        /// <returns></returns>
        public (double, int) GetInputValFromAlgorithm(string data_file_path)
        {
            double column_rate;
            double row_rate;
            // Set Item1 to be 100% different  |   Set Item2 at the current likeliest value
            (double, int) lowest_difference_rate = (100, -1);
            for (int folder_num = 0; folder_num <= 9; folder_num++)
            {
                column_rate = algorithm.CompareInputDataToTrainingData(data_file_path + $"{folder_num}\\Averages\\column.csv", data_file_path + "input_columns.csv");
                row_rate = algorithm.CompareInputDataToTrainingData(data_file_path + $"{folder_num}\\Averages\\row.csv", data_file_path + "input_rows.csv");

                if ((column_rate + row_rate) / 2 < lowest_difference_rate.Item1)
                {
                    lowest_difference_rate.Item1 = (column_rate + row_rate) / 2;
                    lowest_difference_rate.Item2 = folder_num;
                }
            }

            return lowest_difference_rate;
        }

        /// <summary>
        /// CAUTION: This will run through all the saved data and calculate default training data values.
        /// </summary>
        public void ResetTrainingData(string data_file_path)
        {
            logic.GenerateTrainingDataFromTrainingImages(data_file_path);
        }
    }
}
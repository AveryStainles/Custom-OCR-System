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

            // Setup Input Data
            string data_file_path = "C:\\Users\\fox-r\\source\\repos\\AverySecretProject\\Data\\";
            string input_image_file_path = data_file_path + "ImageInput.png";
            logic.GenerateTrainingDataFromTrainingImages(data_file_path);
            var data = logic.GetImageData(input_image_file_path);
            SaveInputData(data_file_path + "input_rows.csv", String.Join(",", data.Item1));
            SaveInputData(data_file_path + "input_columns.csv", String.Join(",", data.Item2));


            lblRowData.Text = "Val | Difference Rate:";

            var lowest_difference_rate = GetInputVal(data_file_path);
            RenderInputImage(input_image_file_path);
            lblRowData.Text += "\n   " + lowest_difference_rate.Item2 + " | " + Math.Round(lowest_difference_rate.Item1, 3);
        }


        public void RenderInputImage(string img_file_path) { lbl_DisplayText.Text = logic.RenderImage(img_file_path); }


        public void SaveInputData(string img_file_path, string data) { helperClass.WriteToFile(img_file_path, data); }


        public (double, int) GetInputVal(string data_file_path)
        {
            // Analyze input data
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
    }
}
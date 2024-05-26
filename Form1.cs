using System.Drawing;

namespace AverySecretProject
{
    public partial class Form1 : Form
    {

        private Read_Write_HelperClass helperClass = new();
        private Logic logic = new();
        private Logic.Algorithm algorithm = new();
        // Setup Paths
        public static string data_file_path { get; set; } = "C:\\Users\\fox-r\\source\\repos\\AverySecretProject\\Data\\";
        public string input_image_file_path { get; set; } = data_file_path + "ImageInput.png";
        public string training_data_path { get; set; } = data_file_path + "0\\Averages\\";

        public Color secondary_colour = ColorTranslator.FromHtml("#170013");
        public Color primary_colour = ColorTranslator.FromHtml("#11000F");
        public Color text_colour = ColorTranslator.FromHtml("#F2EBFF");
        public Color accent_colour = ColorTranslator.FromHtml("#302938");
        public Color accent_colour2 = ColorTranslator.FromHtml("#F2EBFF");
        public Form1()
        {
            InitializeComponent();

            // Colour UI components
            BackColor = primary_colour;
            lbl_DisplayText.BackColor = accent_colour;
            lblData.BackColor = accent_colour;
            lbl_Info.BackColor = accent_colour;

            lbl_DisplayText.ForeColor = accent_colour2;
            lblData.ForeColor = accent_colour2;
            lbl_Info.ForeColor = accent_colour2;

            menu_option.BackColor = accent_colour;
            btn_accuracy_test.BackColor = accent_colour2;
            btn_analyze_input_data.BackColor = accent_colour2;
            btn_render_image_from_data.BackColor = accent_colour2;
            btn_reset_data.BackColor = accent_colour2;

            optionToolStripMenuItem.ForeColor = text_colour;
            btn_accuracy_test.ForeColor = primary_colour;
            btn_analyze_input_data.ForeColor = primary_colour;
            btn_render_image_from_data.ForeColor = primary_colour;
            btn_reset_data.ForeColor = primary_colour;

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
            lblData.Text = $"Row Average: {Math.Round(row_data.Average(), 3)}\nCol Average: {Math.Round(column_data.Average(), 3)}" + "\n\n";
            for (int index = 0; index < row_data.Count; index++)
            {
                lblData.Text += ("Row: " + Math.Round(row_data[index], 3)).PadRight(20) + ("Col:" + Math.Round(column_data[index], 3) + "\n");
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
            lblData.Text = "Val | Difference Rate:";
            // Item2 = number it thinks the user wrote      |       Item 1 = difference rate
            lblData.Text += "\n   " + lowest_difference_rate.Item2 + " | " + Math.Round(lowest_difference_rate.Item1, 3);
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
            lblData.Text = $"Total Score: {total_score}%" + result;
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

        private void SetupInfoLabel(string message)
        {
            lbl_Info.MaximumSize = new Size(lbl_DisplayText.Size.Width, lbl_DisplayText.Height / 3);
            lbl_Info.Top = lbl_DisplayText.Size.Height + 20;
            lbl_Info.Text = message;
        }

        private void btn_render_image_from_data_Click(object sender, EventArgs e)
        {
            try
            {
                Demo_RenderImageFromTestData(training_data_path);
                SetupInfoLabel("Recover an approximate render of the image from the collected data." + "\nIn this example the render is an average of all the data used to train 0");
            }
            catch (Exception ex)
            {
                lbl_DisplayText.Text = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Text = "";
            }
        }

        private void btn_reset_data_Click(object sender, EventArgs e)
        {
            logic.GenerateTrainingDataFromTrainingImages(data_file_path);
            lbl_DisplayText.Text = "Training Data Has Successfully Backed Up Using Training_Images";

            // TODO: turn this into a pop up
            SetupInfoLabel("CAUTION!\nResets all the Training_Data using the images in Training_Data_Images");
            lblData.Text = "";
        }

        private void btn_analyze_input_data_Click(object sender, EventArgs e)
        {
            try
            {
                Demo_EvaluateInput(input_image_file_path, data_file_path);
                SetupInfoLabel("Looks for ImageInput.png in the Data folder as a user input example. Then it is analyzed and an output is given.");

            }
            catch (Exception ex)
            {
                lbl_DisplayText.Text = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Text = "";
            }
        }

        private void btn_accuracy_test_Click(object sender, EventArgs e)
        {
            try
            {
                RunAccuracyTest(data_file_path);
                SetupInfoLabel("Runs 10 tests for each values the algorithm was trained to recognize (numbers from 0 to 9) using different sample data.");
            }
            catch (Exception ex)
            {
                lbl_DisplayText.Text = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Text = "";
            }
        }

        private void optionToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            optionToolStripMenuItem.ForeColor = primary_colour;
        }

        private void optionToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            optionToolStripMenuItem.ForeColor = text_colour;
        }
    }
}
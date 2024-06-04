using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Input;

namespace Custom_Optical_Character_Recognition_System
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Read_Write_HelperClass helperClass = new Read_Write_HelperClass();
        private Logic logic = new Logic();
        private Logic.Algorithm algorithm = new Logic.Algorithm();
        // Setup Paths
        public static string data_file_path { get; set; } = "C:\\Users\\fox-r\\source\\repos\\AveryStainles\\AveryStainles\\Custom_Optical_Character_Recognition_System\\Data\\";
        public string input_image_file_path { get; set; } = data_file_path + "ImageInput.png";
        public string training_data_path { get; set; } = data_file_path + "0\\Averages\\";

        // Application Colours
        public SolidColorBrush secondary_colour { get; set; }
        public SolidColorBrush primary_colour { get; set; }
        public SolidColorBrush text_colour { get; set; }
        public SolidColorBrush accent_colour { get; set; }
        public SolidColorBrush accent_colour2 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetupColours();
            SetupUiStyling();

        }

        private void SetupColours()
        {
            BrushConverter converter = new BrushConverter();
            secondary_colour = (SolidColorBrush)converter.ConvertFromString("#170013");
            primary_colour = (SolidColorBrush)converter.ConvertFromString("#11000F");
            text_colour = (SolidColorBrush)converter.ConvertFromString("#F2EBFF");
            accent_colour = (SolidColorBrush)converter.ConvertFromString("#302938");
            accent_colour2 = (SolidColorBrush)converter.ConvertFromString("#F2EBFF");

        }

        private void RenderInputImage(string img_file_path) { lbl_DisplayText.Content = logic.RenderImage(img_file_path); }

        private void SaveInputData(string save_location_path, string data) { helperClass.WriteToFile(save_location_path, data); }


        /// <summary>
        /// returns a tuple with the data corresponding to the difference rate and the value the algorithm recognized.
        /// <para>Item 1: (double) Difference Rate</para>
        /// <para>Item 2: (int) Value the algorithm recognized</para>
        /// </summary>
        /// <param name="data_file_path"> Path to the Data folder of the project as a string</param>
        /// <returns></returns>
        private (double, int) GetInputValFromAlgorithm(string data_file_path)
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

        private void SetupUiStyling()
        {
            // Colour UI components
            lbl_DisplayText.Foreground = accent_colour2;
            lblData.Foreground = accent_colour2;
            lbl_Info.Foreground = accent_colour2;


            // TODO: Make Canvas Expandable
            grid_display_layer.MinHeight = 450;
            grid_display_layer.MinWidth = 450;
            writting_canvas.MaxHeight = 1000;
            writting_canvas.MaxWidth = 1000;
        }

        private void SetupInfoLabel(string message)
        {
            lbl_Info.Text = message;
        }

        // Clears all the content for visual components
        private void ClearUI()
        {
            lblData.Content = "";
            lbl_DisplayText.Content = "";
            lbl_Info.Text = "";

            writting_canvas.IsEnabled = false;
            writting_canvas.Visibility = Visibility.Hidden;
        }

        private void ClearCanvas() { writting_canvas.Children.Clear(); }

        /// <summary>
        /// Draws a circle using the Ellipse Method. Drawn on canvas at mouse coodinates.
        /// </summary>
        /// <param name="mouse_pos"></param>
        private void DrawCircle(System.Windows.Point mouse_pos)
        {
            // Set brush size
            int brush_size = (int)Math.Floor(writting_canvas.Width * 0.09);

            // Create the circle
            Ellipse myEllipse = new Ellipse();
            myEllipse.Stroke = Brushes.Black;
            myEllipse.Fill = Brushes.Black;
            myEllipse.Width = brush_size;
            myEllipse.Height = brush_size;

            // Set location of circle to be at the end of the cursor icon's point.
            Canvas.SetTop(myEllipse, mouse_pos.Y - 20);
            Canvas.SetLeft(myEllipse, mouse_pos.X - 20);

            // Add the circle to the canvas
            writting_canvas.Children.Add(myEllipse);
        }


        /// <summary>
        ///         Setup Content and Demos 
        /// </summary>


        /// <summary>
        /// A quick demo to help render the overal heatmap of the data
        /// </summary>
        private void Demo_RenderImageFromTestData(string training_data_path)
        {
            // Setup Data
            List<double> row_data = helperClass.GetListFromDataPath(training_data_path + "row.csv");
            List<double> column_data = helperClass.GetListFromDataPath(training_data_path + "column.csv");

            // Render Image From Training Data
            double heat_sensitivity_rate = 1.3;
            string test = logic.RenderImageFromData(row_data, column_data, heat_sensitivity_rate);
            lbl_DisplayText.Content = test;

            // Load Data About The Render
            lblData.Content = $"Row Average: {Math.Round(row_data.Average(), 3)}\nCol Average: {Math.Round(column_data.Average(), 3)}" + "\n\n";
            for (int index = 0; index < row_data.Count; index++)
            {
                lblData.Content += ("Row: " + Math.Round(row_data[index], 3)).PadRight(20) + ("Col:" + Math.Round(column_data[index], 3) + "\n");
            }
        }

        /// <summary>
        /// Evaluates input data compared to training data to display the value the system recognized from the input.
        /// </summary>
        /// <param name="input_image_file_path">Full path of the .png file to analyze</param>
        /// <param name="data_file_path">>Path to the Data folder of the project as a string</param>
        private void Demo_EvaluateInput(string input_image_file_path, string data_file_path)
        {
            // Setup data
            // Item 1: row data     Item 2: column data
            (List<double>, List<double>) data = logic.GetImageData(input_image_file_path);
            SaveInputData(data_file_path + "input_rows.csv", String.Join(",", data.Item1));
            SaveInputData(data_file_path + "input_columns.csv", String.Join(",", data.Item2));
            (double, int) lowest_difference_rate = GetInputValFromAlgorithm(data_file_path);

            // Display data and user input image
            RenderInputImage(input_image_file_path);
            lblData.Content = "Val | Difference Rate:";
            // Item2 = number it thinks the user wrote      |       Item 1 = difference rate
            lblData.Content += "\n   " + lowest_difference_rate.Item2 + " | " + Math.Round(lowest_difference_rate.Item1, 3);
        }


        /// <summary>
        /// Performs 10 tests with different inputs on each number. (as of 2024-05-25 this is 100 inputs being tested
        /// </summary>
        /// <param name="data_file_path">Path to the Data folder of the project as a string</param>
        /// <returns></returns>
        private void RunAccuracyTest(string data_file_path)
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
            lblData.Content = $"Total Score: {total_score}%" + result;
        }


        /// <summary>
        ///         This is the start of the event listeners. 
        /// </summary>


        private void btn_render_image_from_data_Click(object sender, EventArgs e)
        {
            ClearUI();
            try
            {
                Demo_RenderImageFromTestData(training_data_path);
                SetupInfoLabel("Recover an approximate render of the image from the collected data." + "\nIn this example the render is an average of all the data used to train 0");
            }
            catch (Exception ex)
            {
                lbl_DisplayText.Content = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Content = "\nEVENT: btn_render_image_from_data_Click\nEXCEPTION: " + ex; ;
            }
        }

        private void btn_reset_data_Click(object sender, EventArgs e)
        {
            ClearUI();
            logic.GenerateTrainingDataFromTrainingImages(data_file_path);
            lbl_DisplayText.Content = "Training Data Has Successfully Backed Up Using Training_Images";

            // TODO: turn this into a pop up

            SetupInfoLabel("Success!");
            lblData.Content = "";
        }

        private void btn_analyze_input_data_Click(object sender, EventArgs e)
        {
            ClearUI();
            try
            {
                Demo_EvaluateInput(input_image_file_path, data_file_path);
                SetupInfoLabel("Looks for ImageInput.png in the Data folder as a user input example. Then it is analyzed and an output is given.");

            }
            catch (Exception ex)
            {
                lbl_DisplayText.Content = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Content = "\nEVENT: btn_analyze_input_data_Click\nEXCEPTION: " + ex; ;
            }
        }

        private void btn_accuracy_test_Click(object sender, EventArgs e)
        {
            ClearUI();
            try
            {
                RunAccuracyTest(data_file_path);
                SetupInfoLabel("Runs 10 tests for each values the algorithm was trained to recognize (numbers from 0 to 9) using different sample data.");
            }
            catch (Exception ex)
            {
                lbl_DisplayText.Content = "Could not run demo. Try reseting the data to the backed up images.";
                lblData.Content = e + "\nEVENT: btn_accuracy_test_Click\nEXCEPTION: " + ex;
            }
        }


        // writting_canvas Events
        private void btn_writting_canvas_Click(object sender, EventArgs e)
        {
            ClearUI();
            lbl_Info.Text = "Write a digit between 0-9 and press the buttons below to use as training data.\nLeft Click = Draw\nRight Click = Clear";
            writting_canvas.IsEnabled = true;
            writting_canvas.Visibility = Visibility.Visible;
            BrushConverter converter = new BrushConverter();
            writting_canvas.Background = (SolidColorBrush)converter.ConvertFromString("#FFFFFF");
        }


        /// <summary>
        /// Creates a circle from the cursor position relative to the canvas.
        /// </summary>
        private void writting_canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point mouse_pos = e.GetPosition(writting_canvas);
                DrawCircle(mouse_pos);
            }
        }

        private void draw_LeftMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DrawCircle(e.GetPosition(writting_canvas));
        }

        private void writting_canvas_RightMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                ClearCanvas();
        }
    }
}
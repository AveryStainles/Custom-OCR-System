using Custom_Optical_Character_Recognition_System.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Custom_Optical_Character_Recognition_System.MVVM.View
{
    /// <summary>
    /// Interaction logic for TrainAlgView.xaml
    /// </summary>
    public partial class TrainAlgView : UserControl
    {
        private List<Button> training_data_buttons { get; set; }
        private DataAlgorithm dataAlgorithm = new DataAlgorithm();
        private CanvasFunctions canvas_helper = new CanvasFunctions();
        private DataAccessPoint dao = new DataAccessPoint();
        public static bool IsPopupOpen { get; set; } = false;

        public TrainAlgView()
        {
            InitializeComponent();
            training_data_buttons = new List<Button>() { training_btn_1, training_btn_2, training_btn_3, training_btn_4 };
            txt_data_report.Text = dao.CreateTrainingDataReport();
            SetupTrainingButtons();
        }

        //Setup training buttons.
        private void SetupTrainingButtons(int data_set_index = 0)
        {
            btn_next_training_data.IsEnabled = (dao._data.Count >= 5);

            // Set the content of each button based off the training data values. If out of bound, leave content blank
            for (int index = 0; index + data_set_index < dao._data.Count && index <= 3; index++)
            {
                training_data_buttons[index].Content = dao._data[index + data_set_index].Value;
                training_data_buttons[index].IsEnabled = true;
            }

            // If there wasn't enough training_values to populate the buttons (button conten was left blank), disable the buttons.
            foreach (Button btn in training_data_buttons)
            {
                if ((btn.Content + "").Length > 0) { continue; }

                btn.IsEnabled = false;
                btn_next_training_data.IsEnabled = false;
            }
        }

        // Clears the canvas on right click
        private void Drawing_Canvas_RightMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) { CanvasFunctions.ClearCanvas(drawing_canvas); }
        }


        // Draws a circle on mouse click
        private void Drawing_Canvas_LeftMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            canvas_helper.DrawCircle(e.GetPosition(drawing_canvas), drawing_canvas);
        }


        // Draws on mouse movement
        private void Drawing_Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse_pos = e.GetPosition(drawing_canvas);
            if (e.LeftButton == MouseButtonState.Pressed && canvas_helper.IsMouseInCanvasBounderies(mouse_pos, drawing_canvas))
            {
                canvas_helper.DrawCircle(mouse_pos, drawing_canvas);
            }
        }

        /// <summary>
        /// Updates training data corresponding to the content property of the clicked button.
        /// Data is updated by reversing the average into a sum, then updated with the new data into a new total average.
        /// </summary>
        private void Train_Data(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"AVERY DEBUG: {((Button)sender).Content} Button was Clicked!");


            (List<double>, List<double>) canvas_input_data = GetImageDataFlow();

            // Update training data with new value
            dao.UpdateTrainingDataByValue(((Button)sender).Content + "", canvas_input_data.Item1, canvas_input_data.Item2);
            txt_data_report.Text = dao.CreateTrainingDataReport();
            lbl_recognized_value.Content = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);
        }


        private void GetNextTrainingButtonsClick(object sender, RoutedEventArgs e)
        {
            // If there is no other trained data to loop to, return
            if (dao._data.Count <= 4) { return; }

            // get value from training_data that matches button content
            int index = dao.GetDataIndexByValue(training_btn_4.Content + "");

            // disable next button if last training_data value
            if (index + 1 == dao._data.Count - 1)
            {
                btn_next_training_data.IsEnabled = false;
            }

            for (int btn_index = 0; btn_index < training_data_buttons.Count; btn_index++)
            {
                // set each buttons to it's next value except for the last one
                if (btn_index + 1 < training_data_buttons.Count)
                {
                    training_data_buttons[btn_index].Content = training_data_buttons[btn_index + 1].Content;
                }
            }

            // sets the 4th training button to it's next
            if (index + 1 < dao._data.Count)
            {
                training_btn_4.Content = dao._data[index + 1].Value;
            }


            btn_previous_training_data.IsEnabled = true;
        }


        private void GetPreviousTrainingButtonsClick(object sender, RoutedEventArgs e)
        {
            // If there is no other trained data to loop to, return
            int index = dao.GetDataIndexByValue(training_btn_1.Content + "") - 1;

            for (int training_button_index = 0; training_button_index < training_data_buttons.Count; training_button_index++)
            {
                training_data_buttons[training_button_index].Content = dao._data[index + training_button_index].Value;
            }

            btn_next_training_data.IsEnabled = true;
            if (index <= 0)
            {
                btn_previous_training_data.IsEnabled = false;
                return;
            }
        }


        private (List<double>, List<double>) GetImageDataFlow()
        {
            CanvasFunctions canvas_funct = new CanvasFunctions();

            // Setup New Canvas Positiong
            Point canvas_starting_pos = new Point(Math.Floor((training_data_view_window.ActualWidth - drawing_canvas.ActualWidth) / 2), Math.Floor((training_data_view_window.ActualHeight - drawing_canvas.ActualHeight) / 2));

            // Convert Canvas to bitmap
            System.Drawing.Bitmap bitmap = canvas_funct.ConvertCanvasToBitmap(drawing_canvas, canvas_starting_pos);

            // Crop content out
            bitmap = canvas_funct.CropBitmap(bitmap);

            // Scale bitmap to 128x128 pixels
            bitmap = canvas_funct.ScaleBitmap(bitmap, 128, 128);

            // Turn bitmap into usable data
            (List<double>, List<double>) bitmap_data = dataAlgorithm.GetImageData(bitmap);

            return bitmap_data;
        }



        /// Functional Buttons 


        private void Btn_OCR_Click(object sender, RoutedEventArgs e)
        {
            (List<double>, List<double>) canvas_input_data = GetImageDataFlow();
            lbl_recognized_value.Content = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);

        }


        private void Btn_Clear_Click(object sender, RoutedEventArgs e) { CanvasFunctions.ClearCanvas(drawing_canvas); }


        private void Btn_New_Training_Category_Click(object sender, RoutedEventArgs e)
        {
            ConfirmNewTrainingData confirm_box = new ConfirmNewTrainingData();
            confirm_box.canvas_data = GetImageDataFlow();
            if (IsPopupOpen == false)
            {
                IsPopupOpen = true;
                confirm_box.ShowDialog();
                Console.WriteLine("I RAN");
            }
            dao = new DataAccessPoint();
            txt_data_report.Text = dao.CreateTrainingDataReport();
            SetupTrainingButtons();
        }


        private void Btn_Undo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextTrainingBtn_FastScroll_Click(object sender, MouseButtonEventArgs e)
        {
            for (int loop = 0; loop <= 3 && btn_next_training_data.IsEnabled; loop++)
            {
                GetNextTrainingButtonsClick(sender, e);
            }
        }

        private void PreviousTrainingBtn_FastScroll_Click(object sender, MouseButtonEventArgs e)
        {
            for (int loop = 0; loop <= 3 && btn_previous_training_data.IsEnabled; loop++)
            {
                GetPreviousTrainingButtonsClick(sender, e);
            }
        }
    }
}

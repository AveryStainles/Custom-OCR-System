using Custom_Optical_Character_Recognition_System.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Custom_Optical_Character_Recognition_System.MVVM.View
{
    /// <summary>
    /// Interaction logic for TrainAlgView.xaml
    /// </summary>
    public partial class TrainAlgView : UserControl
    {
        private DataAlgorithm dataAlgorithm = new DataAlgorithm();
        private CanvasFunctions canvas_helper = new CanvasFunctions();

        public TrainAlgView()
        {
            InitializeComponent();
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
            DataAccessPoint DAO = new DataAccessPoint();

            (List<double>, List<double>) canvas_input_data = SetImageDataFlow();

            // Update training data with new value
            DAO.UpdateTrainingDataByValue(((Button)sender).Content + "", canvas_input_data.Item1, canvas_input_data.Item2);

            //info_textBlock.Text = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);
        }


        /// <summary>
        /// Button Click -> attempt recognizing value on canvas
        /// </summary>
        private void Recognize_Canvas_Value_Click(object sender, RoutedEventArgs e)
        {
            (List<double>, List<double>) canvas_input_data = SetImageDataFlow();
            //info_textBlock.Text = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);
        }


        private (List<double>, List<double>) SetImageDataFlow()
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

        }
        

        private void Btn_Clear_Click(object sender, RoutedEventArgs e) { CanvasFunctions.ClearCanvas(drawing_canvas); }


        private void Btn_New_Training_Category_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Btn_Undo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

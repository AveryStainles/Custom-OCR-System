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
        private void drawing_canvas_RightMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) { CanvasFunctions.ClearCanvas(drawing_canvas); }
        }


        // Draws a circle on mouse click
        private void drawing_canvas_LeftMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            canvas_helper.DrawCircle(e.GetPosition(drawing_canvas), drawing_canvas);
        }

        // Draws on mouse movement
        private void drawing_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse_pos = e.GetPosition(drawing_canvas);
            if (e.LeftButton == MouseButtonState.Pressed && canvas_helper.IsMouseInCanvasBounderies(mouse_pos, drawing_canvas))
            {
                canvas_helper.DrawCircle(mouse_pos, drawing_canvas);
            }
        }


        /// <summary>
        /// Allows for horizontal scrolling with the Mouse Wheel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Horizontal_Scrolling(object sender, MouseWheelEventArgs e)
        {
            // Scroll wheel up, stack panel scroll right | Scroll wheel down, stack panel scroll left
            training_data_scrollbar.ScrollToHorizontalOffset(training_data_scrollbar.HorizontalOffset + Settings_Control.scrolling_speed
                * ((e.Delta > 0) ? 1 : -1));
        }

        private void Train_Data(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"AVERY DEBUG: {((Button)sender).Content} Button was Clicked!");
            DataAccessPoint DAO = new DataAccessPoint();

            (List<double>, List<double>) canvas_input_data = SetImageDataFlow();

            // Update training data with new value
            DAO.UpdateTrainingDataByValue(((Button)sender).Content + "", canvas_input_data.Item1, canvas_input_data.Item2);

            info_textBlock.Text = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);

            Console.WriteLine("\n\t\t--Button Click Report--");
            Console.WriteLine($"AveryDebug: Crop drawing out of bitmap\t{((canvas_input_data.Item1[0] > 0 && canvas_input_data.Item2[0] > 0) ? "Passed" : "Failed")}");
            Console.WriteLine("AveryDebug: Get Bitmap Data\n\t\t" + String.Join(", ", canvas_input_data.Item1) + "\n\t\t" + String.Join(", ", canvas_input_data.Item2));
            Console.WriteLine($"AveryDebug: Save Complete");
        }

        /// <summary>
        /// Button Click -> attempt recognizing value on canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recognize_canvas_value_Click(object sender, RoutedEventArgs e)
        {
            (List<double>, List<double>) canvas_input_data = SetImageDataFlow();
            info_textBlock.Text = dataAlgorithm.RecognizeValueFromData(canvas_input_data.Item1, canvas_input_data.Item2);
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

            // Scale bitmap to 128x128
            bitmap = canvas_funct.ScaleBitmap(bitmap, 128, 128);

            // Turn bitmap into usable data
            (List<double>, List<double>) bitmap_data = dataAlgorithm.GetImageData(bitmap);

            Console.WriteLine($"AveryDebug: Convert Canvas to Bitmap\t{((bitmap.GetType() == typeof(System.Drawing.Bitmap)) ? "Passed" : "Failed")}");
            Console.WriteLine($"AveryDebug: Scale Bitmap\t\t\t\t{((bitmap.Width == 128 && bitmap.Height == 128) ? "Passed" : "Failed")}");
            return bitmap_data;
        }
    }
}

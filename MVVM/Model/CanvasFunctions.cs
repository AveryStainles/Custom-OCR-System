using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    internal class CanvasFunctions
    {
        /// <summary>
        /// Veryifies that the cursor is in the canvas boundaries
        /// </summary>
        public bool IsMouseInCanvasBounderies(Point mouse_pos, Canvas drawing_canvas)
        {
            int brush_size = (int)Math.Floor(drawing_canvas.Width * 0.08);
            if (mouse_pos.X < (0 + brush_size / 2)) { return false; }
            if (mouse_pos.Y < (0 + brush_size / 2)) { return false; }
            if (mouse_pos.X > (drawing_canvas.Width - (brush_size / 2))) { return false; }
            if (mouse_pos.Y > (drawing_canvas.Height - (brush_size / 2))) { return false; }
            return true;
        }

        // Clears drawing_canvas
        public static void ClearCanvas(Canvas drawing_canvas) { drawing_canvas.Children.Clear(); }

        /// <summary>
        /// Draws a circle using the Ellipse Method on canvas at mouse coodinates.
        /// </summary>
        /// <param name="mouse_pos"></param>
        public void DrawCircle(Point mouse_pos, Canvas drawing_canvas)
        {
            // Set brush size
            int brush_size = (int)Math.Floor(drawing_canvas.Width * 0.09);

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
            drawing_canvas.Children.Add(myEllipse);
        }

        ///<summary>
        /// Converts a canvas into a bitmap
        ///</summary>
        /// Source: https://dotnetqueries.wordpress.com/tag/how-to-convert-wpf-canvas-to-bitmap-image-using-c-net/
        ///
        public System.Drawing.Bitmap ConvertCanvasToBitmap(Canvas input_canvas, Point canvas_starting_pos)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)input_canvas.Width,
                (int)input_canvas.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);

            // needed otherwise the image output is black
            input_canvas.Measure(new Size((int)input_canvas.Width, (int)input_canvas.Height));
            renderBitmap.Render(input_canvas);

            // Convert RenderTargetBitmap to Bitmap
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);

            // return canvas as bitmap
            return new System.Drawing.Bitmap(stream);
        }


        public System.Drawing.Bitmap ScaleBitmap(System.Drawing.Bitmap bitmap, int new_width, int new_height)
        {
            return new System.Drawing.Bitmap(bitmap, new_width, new_height);
        }


        private DataAlgorithm data_algs = new DataAlgorithm();
        public System.Drawing.Bitmap CropBitmap(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null || (bitmap.Width == 0 && bitmap.Height == 0)) { return null; }

            System.Drawing.Rectangle crop_area = new System.Drawing.Rectangle();
            var img_data = data_algs.GetImageData(bitmap);
            // Get x, y, width, height

            int decrementing_index = img_data.Item1.Count - 1;
            int incrementing_index = 0;
            while (crop_area.X == 0 || crop_area.Y == 0 || crop_area.Width == 0 || crop_area.Height == 0)
            {

                // setup starting coord    ( X, Y )
                if (img_data.Item1[incrementing_index] > 0 && crop_area.X == 0)
                {
                    crop_area.X = incrementing_index;
                }
                if (img_data.Item2[incrementing_index] > 0 && crop_area.Y == 0)
                {
                    crop_area.Y = incrementing_index;
                }

                // setup crop Height and Width      ( Basically stopping coord )
                if (img_data.Item1[decrementing_index] > 0 && crop_area.Width == 0 && crop_area.X > 0)
                {
                    crop_area.Width = decrementing_index - incrementing_index;
                }
                if (img_data.Item2[decrementing_index] > 0 && crop_area.Height == 0 && crop_area.Y > 0)
                {
                    crop_area.Height = decrementing_index - incrementing_index;
                }

                incrementing_index++;
                decrementing_index--;
            }
            
            return bitmap.Clone(crop_area, bitmap.PixelFormat);
        }
    }
}

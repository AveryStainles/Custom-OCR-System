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
        // Clears drawing_canvas
        public static void ClearCanvas(Canvas drawing_canvas) => drawing_canvas.Children.Clear();

        // Resize bitmap to reduce required computations
        public System.Drawing.Bitmap ScaleBitmap(System.Drawing.Bitmap bitmap, int new_width, int new_height)
            => new System.Drawing.Bitmap(bitmap, new_width, new_height);

        // Veryifies that the cursor is in the canvas boundaries
        public bool IsMouseInCanvasBounderies(Point mouse_pos, Canvas drawing_canvas)
        {
            int brush_size = (int)Math.Floor(drawing_canvas.Width * 0.08) / 2;

            // Left boundary
            if (mouse_pos.X < (brush_size / 2))
                return false;

            //  Top boundary
            if (mouse_pos.Y < (brush_size / 2))
                return false;

            // Right boundary
            if (mouse_pos.X > (drawing_canvas.Width - (brush_size / 2)))
                return false;

            // Bottom boundary
            if (mouse_pos.Y > (drawing_canvas.Height - (brush_size / 2)))
                return false;

            return true;
        }

        // Draws a circle using the Ellipse Method on canvas at mouse coodinates.
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

            // Draw circle on cursor position
            Canvas.SetTop(myEllipse, mouse_pos.Y - 20);
            Canvas.SetLeft(myEllipse, mouse_pos.X - 20);
            drawing_canvas.Children.Add(myEllipse);
        }

        // Converts a canvas into a bitmap
        public System.Drawing.Bitmap ConvertCanvasToBitmap(Canvas input_canvas, Point canvas_starting_pos)
        {
            RenderTargetBitmap renderBitmap =
                new RenderTargetBitmap((int)input_canvas.Width, (int)input_canvas.Height, 96d, 96d, PixelFormats.Pbgra32);

            // needed otherwise the image output is black
            input_canvas.Measure(new Size(input_canvas.Width, input_canvas.Height));
            renderBitmap.Render(input_canvas);

            // Convert RenderTargetBitmap to Bitmap
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);

            // return canvas as bitmap
            return new System.Drawing.Bitmap(stream);
        }
    }
}

using Custom_Optical_Character_Recognition_System.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Custom_Optical_Character_Recognition_System.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

        }

        private void Can_Save_Images_Checked(object sender, RoutedEventArgs e)
        {

            txt_saved_images_path.IsEnabled = ((CheckBox)sender).IsChecked == true;
        }

        private void Btn_OCR_Click(object sender, RoutedEventArgs e)
        {
            btn_upload_ocr.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#5F001F");
        }

        private void Btn_OCR_MouseMove(object sender, MouseEventArgs e)
        {
            btn_upload_ocr.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#291829");
        }
    }
}

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
            scroll_speed_slider.Value = Settings_Control.scrolling_speed;
            lbl_display_scroll_speed.Content = Math.Floor(Settings_Control.scrolling_speed);
        }

        private void scroll_speed_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbl_display_scroll_speed == null) { return; }

            lbl_display_scroll_speed.Content = Math.Floor(e.NewValue);
            Settings_Control.scrolling_speed = e.NewValue;
        }
    }
}

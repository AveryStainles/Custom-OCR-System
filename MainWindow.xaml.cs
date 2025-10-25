using System.Windows;
using System.Windows.Input;
using Custom_Optical_Character_Recognition_System.MVVM.Model;

namespace Custom_Optical_Character_Recognition_System
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataAccessPoint dao = new DataAccessPoint();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
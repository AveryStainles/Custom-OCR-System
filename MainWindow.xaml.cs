using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
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
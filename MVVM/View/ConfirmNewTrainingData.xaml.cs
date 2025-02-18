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
using System.Windows.Shapes;

namespace Custom_Optical_Character_Recognition_System.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConfirmNewTrainingData.xaml
    /// </summary>
    public partial class ConfirmNewTrainingData : Window
    {
        public static string new_training_value { get; set; } = "";
        public (List<double>, List<double>) canvas_data { get; set; } = (null, null);
        public ConfirmNewTrainingData()
        {
            InitializeComponent();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            new_training_value = "";
            TrainAlgView.IsPopupOpen = false;
            Close();
        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            new_training_value = txt_user_input.Text;
            if (canvas_data.Item1 == null || canvas_data.Item2 == null)
            {
                Console.WriteLine("New data was null");
                return;
            }

            if (new_training_value.Length == 0)
            {
                Console.WriteLine("No new value specified. Make sure to enter a character");
                return;
            }

            DataAccessPoint dao = new DataAccessPoint();
            dao._data.Add(DataAccessPoint.Create_Training_Data(ConfirmNewTrainingData.new_training_value, canvas_data.Item1, canvas_data.Item2));
            dao.SaveAllTrainingData();
            

            TrainAlgView.IsPopupOpen = false;
            Close();
        }
    }
}

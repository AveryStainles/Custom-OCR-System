using Custom_Optical_Character_Recognition_System.Core;

namespace Custom_Optical_Character_Recognition_System.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        // Relay Commands
        public RelayCommand TrainAlgViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand ExitApplicationCommand { get; set; }

        // View Models
        private TrainAlgViewModel TrainAlgVM { get; set; }
        private SettingsViewModel SettingsVM { get; set; }
        private object _currentView;


        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TrainAlgVM = new TrainAlgViewModel();
            SettingsVM = new SettingsViewModel();
            CurrentView = TrainAlgVM;

            TrainAlgViewCommand = new RelayCommand(obj =>
            {
                CurrentView = TrainAlgVM;
            });

            SettingViewCommand = new RelayCommand(obj =>
            {
                CurrentView = SettingsVM;
            });

            ExitApplicationCommand = new RelayCommand(obj =>
            {
                // TODO: Add popupasking if they are sure
                System.Windows.Application.Current.Shutdown();
            });
        }
    }
}

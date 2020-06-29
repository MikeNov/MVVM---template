using System.Windows;

namespace MVVMSQLiteApp
{
    public partial class UserDataView : Window
    {
        public UserData UserDataModel { get; private set; }

        public UserDataView(UserData uD)
        {
            InitializeComponent();
            UserDataModel = uD;
            this.DataContext = UserDataModel;
        }

        private void Ok_click(object sender, RoutedEventArgs rEA)
        {
            this.DialogResult = true;
        }

        public void UpdateButtons(bool state)
        {
            MainView mV = Application.Current.MainWindow as MainView;
            if (mV != null)
            {
                mV.ChangeButton.IsEnabled = state;
                mV.DeleteButton.IsEnabled = state;
            }
        }
    }
}
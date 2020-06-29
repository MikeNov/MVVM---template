using System.Windows;

namespace MVVMSQLiteApp
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new UserViewModel();
        }
    }
}
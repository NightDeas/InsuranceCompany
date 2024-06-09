using System.Configuration;
using System.Data;
using System.Windows;

namespace InsuranceCompany
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow MainWindow;
        public static Entities.User User;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            MainWindow = new MainWindow();
            MainWindow.MainFrame.Navigate(new Pages.LoginPage());
            MainWindow.ShowDialog();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show($"{e.Exception.Message}", "Критическая ошибка");
        }
    }

}

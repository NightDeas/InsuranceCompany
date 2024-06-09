using InsuranceCompany.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InsuranceCompany
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.LoginPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.Content.GetType() == typeof(Pages.LoginPage))
                BackBtn.Visibility = Visibility.Collapsed;
            else
                BackBtn.Visibility = Visibility.Visible;
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientsPage());
        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeePage());

        }

        private void RiskBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RiskPage());
        }

        private void PolicyTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PolicyTypesPage());

        }

        private void PolicyBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PolicyPage());
        }

        private void AccountBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalRoomPage());
        }
    }
}
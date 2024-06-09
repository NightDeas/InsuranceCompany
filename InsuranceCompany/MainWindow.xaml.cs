using InsuranceCompany.Entities;
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

		public void GetAccess()
		{
			switch (App.User.RoleId)
			{
				case 1:
					AccountBtn.Visibility = Visibility.Visible;
					ClientBtn.Visibility = Visibility.Collapsed;
					EmployeeBtn.Visibility = Visibility.Collapsed;
					GroupClientBtn.Visibility = Visibility.Collapsed;
					PolicyTypeBtn.Visibility = Visibility.Collapsed;
					RiskBtn.Visibility = Visibility.Collapsed;
					PolicyBtn.Visibility = Visibility.Collapsed;
					break;
				case 2:
					AccountBtn.Visibility = Visibility.Visible;
					ClientBtn.Visibility = Visibility.Visible;
					EmployeeBtn.Visibility = Visibility.Collapsed;
					GroupClientBtn.Visibility = Visibility.Collapsed;
					PolicyTypeBtn.Visibility = Visibility.Collapsed;
					RiskBtn.Visibility = Visibility.Collapsed;
					PolicyBtn.Visibility = Visibility.Visible;
					break;
				case 3:
					AccountBtn.Visibility = Visibility.Visible;
					ClientBtn.Visibility = Visibility.Visible;
					EmployeeBtn.Visibility = Visibility.Visible;
					GroupClientBtn.Visibility = Visibility.Visible;
					PolicyTypeBtn.Visibility = Visibility.Visible;
					RiskBtn.Visibility = Visibility.Visible;
					PolicyBtn.Visibility = Visibility.Visible;
					break;

			}
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

		private void GroupClientBtn_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Pages.GroupClientPage());
		}
	}
}
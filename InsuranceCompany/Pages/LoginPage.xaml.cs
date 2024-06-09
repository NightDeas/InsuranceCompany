using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

namespace InsuranceCompany.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            App.MainWindow.MenuStackPanel.Visibility = Visibility.Collapsed;
            App.MainWindow.MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
         


            Entities.Context context = new();
            var user = context.Users
                .Include(x=> x.Genre)
                .Include(x=> x.Role)
                .FirstOrDefault(x => x.Login == LoginTb.Text && x.Password == PasswordPb.Password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            App.User = user;
            App.MainWindow.MainFrame.Navigate(new Pages.MenuPage());
            App.MainWindow.MenuStackPanel.Visibility = Visibility.Visible;
            App.MainWindow.MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
            App.MainWindow.GetAccess();

        }
    }
}

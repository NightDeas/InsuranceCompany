using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditClientPage.xaml
    /// </summary>
    public partial class EditClientPage : Page
    {
        Entities.Client Client = new();
        public EditClientPage(Entities.Client client)
        {
            InitializeComponent();
            Client = client;
            if (Client == null)
            {
                Client = new();
                Client.User = new();
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Visible;
            }
            DataContext = Client;
            if (Client.User.DateOfBirth == DateTime.MinValue)
            {
                Client.User.DateOfBirth = DateTime.Now;

            }
            LoadData();
        }

        private void LoadData()
        {
            GenreCb.ItemsSource = null;
            GroupRiskCb.ItemsSource = null;
            Entities.Context context = new();
            GenreCb.ItemsSource = context.Genres.ToList();
            GroupRiskCb.ItemsSource = context.ClientGroups.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Client.GroupId == 0 ||
                string.IsNullOrEmpty(Client.User.LastName) ||
                string.IsNullOrEmpty(Client.User.FirstName) ||
                string.IsNullOrEmpty(Client.User.Telephone) ||
                string.IsNullOrEmpty(Client.User.Address) ||
                string.IsNullOrEmpty(Client.User.Login) ||
                string.IsNullOrEmpty(Client.User.Passport) ||
                Client.User.DateOfBirth == DateTime.MinValue ||
                Client.User.GenreId == 0)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            if (Client.User.Login.Length < 3 || Client.User.Password.Length < 3)
            {
                MessageBox.Show("Длина логина и пароль не может быть меньше 3 символов");
                return;
            }
            Entities.Context context = new();
            bool find = context.Users.Any(x => x.Login == Client.User.Login && x.Id != Client.UserId);
            if (find)
            {
                MessageBox.Show("Данный логин уже занят");
                return;
            }

            Client.User.RoleId = 1;
            if (Client.Id == 0)
            {
                context.Entry(Client).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                context.Entry(Client.User).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                context.Entry(Client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.Entry(Client.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            }

            try
            {
                context.SaveChanges();
                MessageBox.Show("Клиент сохранен");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
            App.MainWindow.MainFrame.GoBack();
            LoadData();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (App.User.RoleId != 3)
            {
                MessageBox.Show("Недостаточно прав для удаления клиента");
                return;
            }
            var result = MessageBox.Show("Действительно удалить?", "Предупреждение", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            Entities.Context context = new();
            Client.User.IsActive = false;
            context.Entry(Client.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                context.SaveChanges();
                MessageBox.Show("Клиент удален");

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
            App.MainWindow.MainFrame.GoBack();
            LoadData();

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.GoBack();
            LoadData();
        }

        private void TextBoxOnlyLetters_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex isLetters = new(@"[а-яa-z]");
            if (!isLetters.IsMatch(e.Text.ToLower()))
                e.Handled = true;
        }

        private void TextBoxTelephone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex isNumberTelephone = new(@"[\d-+\s()]");
            if (!isNumberTelephone.IsMatch(e.Text.ToLower()))
                e.Handled = true;
        }

        private void TextBoxPassport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex isNumber = new(@"\d");
            if ((sender as TextBox).Text.Length > 10 || !isNumber.IsMatch(e.Text))
                e.Handled = true;
        }
    }
}

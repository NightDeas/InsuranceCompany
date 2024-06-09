using InsuranceCompany.Entities;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для EditEmployeePage.xaml
    /// </summary>
    public partial class EditEmployeePage : Page
    {
        Employee Employee;
        public EditEmployeePage(Entities.Employee employee)
        {
            InitializeComponent();
            Employee = employee;
            if (Employee == null)
            {
                Employee = new();
                Employee.User = new();
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Visible;
            }
            DataContext = Employee;
                if (Employee.User.DateOfBirth == DateTime.MinValue)
                    Employee.User.DateOfBirth = DateTime.Now;

            LoadData();
        }

        private void LoadData()
        {
            Context context = new();
            GenreCb.ItemsSource = context.Genres.ToList();
            PostCb.ItemsSource = context.Posts.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Employee.PostId == 0 ||
                string.IsNullOrEmpty(Employee.User.LastName) ||
                string.IsNullOrEmpty(Employee.User.FirstName) ||
                string.IsNullOrEmpty(Employee.User.Telephone) ||
                string.IsNullOrEmpty(Employee.User.Address) ||
                string.IsNullOrEmpty(Employee.User.Login) ||
                string.IsNullOrEmpty(Employee.User.Passport) ||
                Employee.User.DateOfBirth == DateTime.MinValue ||
                Employee.User.GenreId == 0)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            if (Employee.User.Login.Length < 3 || Employee.User.Password.Length < 3)
            {
                MessageBox.Show("Длина логина и пароль не может быть меньше 3 символов");
                return;
            }
            Entities.Context context = new();
            bool find = context.Users.Any(x => x.Login == Employee.User.Login && x.Id != Employee.UserId);
            if (find)
            {
                MessageBox.Show("Данный логин уже занят");
                return;
            }

            Employee.User.RoleId = 2;
            if (Employee.Id == 0)
            {
                context.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                context.Entry(Employee.User).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                context.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.Entry(Employee.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            try
            {
                context.SaveChanges();
                MessageBox.Show("Сотрудник сохранен");
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
            var result = MessageBox.Show("Действительно удалить?", "Предупреждение", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            Entities.Context context = new();
            Employee.User.IsActive = false;
            context.Entry(Employee.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                context.SaveChanges();
                MessageBox.Show("Сотрудник удален");

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

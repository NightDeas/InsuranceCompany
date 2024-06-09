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
    /// Логика взаимодействия для PolicyPageEdit.xaml
    /// </summary>
    public partial class EditPolicyPage : Page
    {
        Entities.Policy Policy;
        public EditPolicyPage(Entities.Policy policy)
        {
            InitializeComponent();
            Policy = policy;
            if (Policy == null)
            {
                Policy = new();
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Visible;
            }
            DataContext = Policy;
            if (Policy.StartDate == DateTime.MinValue)
            {
                Policy.StartDate = DateTime.Now;
                Policy.EndDate = DateTime.Now;
            }
            LoadData();
        }

        private void LoadData()
        {
            TypesCb.ItemsSource = null;
            ClientsCb.ItemsSource = null;
            EmployeeCb.ItemsSource = null;
            Entities.Context context = new();
            TypesCb.ItemsSource = context.PolicyTypes.ToList();
            ClientsCb.ItemsSource = context.Clients
                .Include(x => x.User)
                .Where(x => x.User.RoleId == 1)
                .ToList();
            EmployeeCb.ItemsSource = context.Employees
                .Include(x => x.User)
                .Where(x => x.User.RoleId == 2)
                .ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Policy.TypeId == 0 ||
                string.IsNullOrEmpty(Policy.Number) ||
              Policy.ClientId == 0 ||
                 Policy.EmployeeId == 0 ||
                Policy.StartDate == DateTime.MinValue ||
                Policy.EndDate == DateTime.MinValue ||
                Policy.Price == 0 ||
                Policy.PaymentAmount == 0)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            if (Policy.StartDate > Policy.EndDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания");
                return;
            }
            if (Policy.Price < 1000 || Policy.PaymentAmount < 1000)
            {
                MessageBox.Show("Цена полиса и сумма выплаты не может быть меньше 1000");
                return;
            }
            Entities.Context context = new();
            bool find = context.Policies.Any(x => x.Number == Policy.Number && x.Id != Policy.Id);
            if (find)
            {
                MessageBox.Show("Данный номер уже существует в базе");
                return;
            }
            if (Policy.Number.Length != 16)
            {
                MessageBox.Show("Номер должен состоять из 16 цифр");
                return;
            }

            if (Policy.Id == 0)
            {
                context.Entry(Policy).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                context.Entry(Policy).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            try
            {
                context.SaveChanges();
                MessageBox.Show("Полис сохранен");
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
            context.Entry(Policy).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                MessageBox.Show("Полис удален");

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

        private void TextBoxOnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex isNumber = new(@"\d");
            if ((sender as TextBox).Text.Length > 15 || !isNumber.IsMatch(e.Text))
                e.Handled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

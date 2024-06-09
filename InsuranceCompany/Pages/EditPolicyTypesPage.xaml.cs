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
    /// Логика взаимодействия для EditPolicyTypesPage.xaml
    /// </summary>
    public partial class EditPolicyTypesPage : Page
    {
        Entities.PolicyType PolicyType;
        public EditPolicyTypesPage(Entities.PolicyType policyType)
        {
            InitializeComponent();
            PolicyType = policyType;
            if (PolicyType == null)
                PolicyType = new();
            DataContext = PolicyType;
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty(PolicyType.Name) ||
                string.IsNullOrEmpty(PolicyType.Description) ||
                string.IsNullOrEmpty(PolicyType.Conditions))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            Entities.Context context = new();
            bool find = context.PolicyTypes.Any(x => x.Name == PolicyType.Name && x.Id != PolicyType.Id);
            if (find)
            {
                MessageBox.Show("Данный вид полиса уже существует в системе");
                return;
            }

            if (PolicyType.Id == 0)
            {
                context.Entry(PolicyType).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                context.Entry(PolicyType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            try
            {
                context.SaveChanges();
                MessageBox.Show("Вид полиса сохранен");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
            App.MainWindow.MainFrame.GoBack();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Действительно удалить?", "Предупреждение", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            Entities.Context context = new();
            context.Entry(PolicyType).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                MessageBox.Show("Вид полиса удален");

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
            App.MainWindow.MainFrame.GoBack();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.GoBack();
        }

        private void TextBoxAverage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex numbers = new(@"[\d.]");
            if (!numbers.IsMatch(e.Text.ToLower()))
                e.Handled = true;

        }

        private void TextBoxOnlyLetters_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex onlyLetter = new(@"[а-яa-z]");
            if (!onlyLetter.IsMatch(e.Text.ToLower()))
                e.Handled = true;

        }
    }
}

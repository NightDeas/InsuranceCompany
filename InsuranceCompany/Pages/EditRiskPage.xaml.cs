using InsuranceCompany.Entities;
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
    /// Логика взаимодействия для EditRiskPage.xaml
    /// </summary>
    public partial class EditRiskPage : Page
    {
        Entities.Risk Risk;
        public EditRiskPage(Entities.Risk risk)
        {
            InitializeComponent();
            Risk = risk;
            if (Risk == null)
            {
                Risk = new();
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
				DeleteBtn.Visibility = Visibility.Visible;
			}
            DataContext = Risk;
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty(Risk.Name) ||
                string.IsNullOrEmpty(Risk.Description) ||
                string.IsNullOrEmpty(Risk.AverageProbability.ToString()))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            if (Risk.AverageProbability < 0 || Risk.AverageProbability > 1)
            {
                MessageBox.Show("Вероятность не может быть больше 1 или меньше 0");
                return;
            }
            Entities.Context context = new();
            bool find = context.Risks.Any(x => x.Name== Risk.Name && x.Id != Risk.Id);
            if (find)
            {
                MessageBox.Show("Данный риск уже существует в системе");
                return;
            }

            if (Risk.Id == 0)
            {
                context.Entry(Risk).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                context.Entry(Risk).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            try
            {
                context.SaveChanges();
                MessageBox.Show("Риск сохранен");
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
            context.Entry(Risk).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                MessageBox.Show("Риск удален");

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



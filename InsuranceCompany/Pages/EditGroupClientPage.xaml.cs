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
	/// Логика взаимодействия для EditGroupClientPage.xaml
	/// </summary>
	public partial class EditGroupClientPage : Page
	{
		Entities.ClientGroup Groupclient;
		public EditGroupClientPage(Entities.ClientGroup risk)
		{
			InitializeComponent();
			Groupclient = risk;
			if (Groupclient == null)
			{
				Groupclient = new();
				DeleteBtn.Visibility = Visibility.Collapsed;
			}
			else
			{
				DeleteBtn.Visibility = Visibility.Visible;
			}
			DataContext = Groupclient;
		}


		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			if (
				string.IsNullOrEmpty(Groupclient.Name) ||
				string.IsNullOrEmpty(Groupclient.Description))
			{
				MessageBox.Show("Не все поля заполнены");
				return;
			}
			Entities.Context context = new();
			bool find = context.Risks.Any(x => x.Name == Groupclient.Name && x.Id != Groupclient.Id);
			if (find)
			{
				MessageBox.Show("Данная группа клиентов уже существует в системе");
				return;
			}

			if (Groupclient.Id == 0)
			{
				context.Entry(Groupclient).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			}
			else
			{
				context.Entry(Groupclient).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			}

			try
			{
				context.SaveChanges();
				MessageBox.Show("Группа клиентов сохранена");
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
			context.Entry(Groupclient).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			try
			{
				context.SaveChanges();
				MessageBox.Show("Группа клиентов удалена");

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

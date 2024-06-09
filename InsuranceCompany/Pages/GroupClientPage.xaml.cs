using InsuranceCompany.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
	/// Логика взаимодействия для GroupClientPage.xaml
	/// </summary>
	public partial class GroupClientPage : Page
	{
		Sort _sort = Sort.None;
		private List<Entities.ClientGroup> _groupsClients;
		private List<Entities.ClientGroup> _allGroupClients;
		public GroupClientPage()
		{
			InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			Entities.Context context = new();
			_allGroupClients = context.ClientGroups.ToList();
			_groupsClients = _allGroupClients;
			GenerateTable();
		}

		private async Task LoadData(string search)
		{
			if (string.IsNullOrEmpty(search))
			{
				LoadData();
				return;
			}
			Entities.Context context = new();
			_allGroupClients = await context.ClientGroups
				.Where(x =>
				x.Name.Contains(search) ||
				x.Description.Contains(search))
				.ToListAsync();
			_groupsClients = _allGroupClients;
			GenerateTable();
		}

		private void GenerateTable()
		{
			MainDataGrid.ItemsSource = null;
			MainDataGrid.ItemsSource = _groupsClients;
		}

		private List<Entities.ClientGroup> SortList()
		{

			switch (_sort)
			{
				case Sort.None:
					return _groupsClients = _groupsClients.OrderBy(x => x.Id).ToList();
				case Sort.NameUp:
					return _groupsClients = _groupsClients.OrderBy(x => x.Name).ToList();
				case Sort.NameDown:
					return _groupsClients = _groupsClients.OrderByDescending(x => x.Name).ToList();
				default:
					throw new Exception("Нету сортировки");
			}
		}

		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			App.MainWindow.MainFrame.Navigate(new Pages.EditGroupClientPage(null));

		}

		private void EditBtn_Click(object sender, RoutedEventArgs e)
		{
			App.MainWindow.MainFrame.Navigate(new Pages.EditGroupClientPage((sender as System.Windows.Controls.Button).DataContext as Entities.ClientGroup));
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadData();
		}

		private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int index = (sender as ComboBox).SelectedIndex;
			switch (index)
			{
				case 0:
					_sort = Sort.None;
					break;
				case 1:
					_sort = Sort.NameUp;
					break;
				case 2:
					_sort = Sort.NameDown;
					break;
				case 3:
					_sort = Sort.AverageUp;
					break;
				case 4:
					_sort = Sort.AverageDown;
					break;
			}
			SortList();
			GenerateTable();
		}

		enum Sort
		{
			None,
			NameUp,
			NameDown,
			AverageUp,
			AverageDown,

		}

		private async void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
		{
			await LoadData(SearchTb.Text);
		}

		private void Page_Loaded_1(object sender, RoutedEventArgs e)
		{

		}
	}
}

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
    /// Логика взаимодействия для PolicyTypesPage.xaml
    /// </summary>
    public partial class PolicyTypesPage : Page
    {
        Sort _sort = Sort.None;
        private List<PolicyType> _types;
        private List<PolicyType> _allTypes;
        public PolicyTypesPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Entities.Context context = new();
            _allTypes = context.PolicyTypes.ToList();
            _types = _allTypes;
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
            _allTypes = await context.PolicyTypes
                .Where(x =>
                x.Name.Contains(search) ||
                x.Description.Contains(search) ||
                x.Conditions.Contains(search))
                .ToListAsync();
            _types = _allTypes;
            GenerateTable();
        }

        private void GenerateTable()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _types;
        }

        private List<Entities.PolicyType> SortList()
        {

            switch (_sort)
            {
                case Sort.None:
                    return _types = _types.OrderBy(x => x.Id).ToList();
                case Sort.NameUp:
                    return _types = _types.OrderBy(x => x.Name).ToList();
                case Sort.NameDown:
                    return _types = _types.OrderByDescending(x => x.Name).ToList();
                default:
                    throw new Exception("Нету сортировки");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditPolicyTypesPage(null));

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditPolicyTypesPage((sender as System.Windows.Controls.Button).DataContext as PolicyType));
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
            }
            SortList();
            GenerateTable();
        }

        enum Sort
        {
            None,
            NameUp,
            NameDown,

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

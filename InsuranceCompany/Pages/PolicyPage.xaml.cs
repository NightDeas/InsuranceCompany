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
    /// Логика взаимодействия для PolicyPage.xaml
    /// </summary>
    public partial class PolicyPage : Page
    {
        Sort _sort = Sort.None;
        private List<Policy> _policies;
        private List<Policy> _allPolicies;
        private int _typeFilterId;
        private int _clientFilterId;
        private int _employeeFilterId;
        public PolicyPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Entities.Context context = new();
            TypeFilterCb.ItemsSource = context.PolicyTypes.ToList();
            EmployeeFilterCb.ItemsSource = context.Users.Where(x=> x.RoleId == 2).ToList();
            ClientFilterCb.ItemsSource = context.Users.Where(x => x.RoleId == 1).ToList();
            _allPolicies = context.Policies
                .Include(x => x.Type)
                .Include(x => x.Client)
                .ThenInclude(x => x.User)
                 .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .ToList();
            _policies = _allPolicies;
            FilterList();
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
            TypeFilterCb.ItemsSource = await context.PolicyTypes.ToListAsync();
            _allPolicies = await context.Policies
                .Include(x => x.Type)
                .Include(x => x.Client)
                .ThenInclude(x => x.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .Where(x =>
                x.Number.Contains(search) ||
                x.Client.User.LastName.Contains(search) ||
                x.Client.User.FirstName.Contains(search) ||
                x.Client.User.Patronymic.Contains(search) ||
                x.Employee.User.FirstName.Contains(search) ||
                x.Employee.User.Patronymic.Contains(search) ||
                x.Employee.User.LastName.Contains(search))
                .ToListAsync();
            _policies = _allPolicies;
            FilterList();
            GenerateTable();
        }

        private void GenerateTable()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _policies;
        }


        private void FilterList()
        {
            _policies = _allPolicies;
            if (_typeFilterId != 0)
                _policies = _policies.Where(x => x.TypeId == _typeFilterId).ToList();
            if (_clientFilterId != 0)
                _policies = _policies.Where(x => x.Client.UserId == _clientFilterId).ToList();
            if (_employeeFilterId != 0)
                _policies = _policies.Where(x => x.Employee.UserId == _employeeFilterId).ToList();
            SortList();
        }

        private List<Entities.Policy> SortList()
        {
            switch (_sort)
            {
                case Sort.None:
                    return _policies = _policies.OrderBy(x => x.Id).ToList();
                case Sort.NumberUp:
                    return _policies = _policies.OrderBy(x => x.Number).ToList();
                case Sort.NumberDown:
                    return _policies = _policies.OrderByDescending(x => x.Number).ToList();
                case Sort.StartDateUp:
                    return _policies = _policies.OrderBy(x => x.StartDate).ToList();
                case Sort.StartDateDown:
                    return _policies = _policies.OrderByDescending(x => x.StartDate).ToList();
                case Sort.EndDateUp:
                    return _policies = _policies.OrderBy(x => x.EndDate).ToList();
                case Sort.EndDateDown:
                    return _policies = _policies.OrderByDescending(x => x.EndDate).ToList();
                default:
                    return _policies = _policies.OrderBy(x => x.Id).ToList();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditPolicyPage(null));

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditPolicyPage((sender as System.Windows.Controls.Button).DataContext as Policy));
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
                    _sort = Sort.NumberUp;
                    break;
                case 2:
                    _sort = Sort.NumberDown;
                    break;
                case 3:
                    _sort = Sort.StartDateUp;
                    break;
                case 4:
                    _sort = Sort.StartDateDown;
                    break;
                case 5:
                    _sort = Sort.EndDateUp;
                    break;
                case 6:
                    _sort = Sort.EndDateDown;
                    break;
            }
            SortList();
            GenerateTable();
        }

        enum Sort
        {
            None,
            NumberUp,
            NumberDown,
            StartDateUp,
            StartDateDown,
            EndDateUp,
            EndDateDown

        }
        private void TypeFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeFilterCb.SelectedItem == null)
                return;
            _typeFilterId = (TypeFilterCb.SelectedItem as PolicyType).Id;
            FilterList();
            GenerateTable();
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            TypeFilterCb.SelectedItem = null;
            _typeFilterId = 0;
            _clientFilterId = 0;
            _employeeFilterId = 0;
            SearchTb.Text = "";
            LoadData();
        }

        private async void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadData(SearchTb.Text);
        }

        private void EmployeeFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeFilterCb.SelectedItem == null) return;
            _employeeFilterId = (EmployeeFilterCb.SelectedItem as User).Id;
            FilterList();
            GenerateTable();
        }

        private void ClientFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientFilterCb.SelectedItem == null) return;
            _clientFilterId = (ClientFilterCb.SelectedItem as User).Id;
            FilterList();
            GenerateTable();
        }


    }
}


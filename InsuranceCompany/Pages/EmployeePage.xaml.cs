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
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {

        Sort _sort = Sort.None;
        private List<Employee> _employees;
        private List<Employee> _allEmployees;
        private int _genreFilterId;
        private int _postFilterId;
        public EmployeePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Entities.Context context = new();
            GenreFilterCb.ItemsSource = context.Genres.ToList();
            PostFilterCb.ItemsSource = context.Posts.ToList();
            _allEmployees = context.Employees
                .Include(x => x.Post)
                .Include(x => x.User)
                .ThenInclude(x => x.Genre)
                .Where(x => (x.User.RoleId == 2 || x.User.RoleId == 3) && x.User.IsActive == true)
                .ToList();
            _employees = _allEmployees;
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
            GenreFilterCb.ItemsSource = await context.Genres.ToListAsync();
            PostFilterCb.ItemsSource = await context.Posts.ToListAsync();
            _allEmployees = await context.Employees
                .Include(x => x.Post)
                .Include(x => x.User)
                .ThenInclude(x => x.Genre)
                .Where(x => (x.User.RoleId == 2 || x.User.RoleId == 3) && x.User.IsActive == true &&(
                x.User.LastName.Contains(search) ||
                x.User.FirstName.Contains(search) ||
                x.User.Patronymic.Contains(search) ||
                x.User.Address.Contains(search) ||
                x.User.Telephone.Contains(search) ||
                x.User.Genre.Name.Contains(search) ||
                x.Post.Name.Contains(search) ||
                x.User.Passport.Contains(search)))
                .ToListAsync();
            _employees = _allEmployees;
            FilterList();
            GenerateTable();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditEmployeePage(null));

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditEmployeePage((sender as Button).DataContext as Employee));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void FilterList()
        {
            _employees = _allEmployees;
            if (_genreFilterId != 0)
                _employees = _employees.Where(x => x.User.GenreId == _genreFilterId).ToList();
            if (_postFilterId != 0)
                _employees = _employees.Where(x => x.PostId == _postFilterId).ToList();
            SortList();
        }
        private List<Entities.Employee> SortList()
        {

            switch (_sort)
            {
                case Sort.None:
                    return _employees = _employees.OrderBy(x => x.Id).ToList();
                case Sort.fioUp:
                    return _employees = _employees.OrderBy(x => x.User.LastName).ToList();
                case Sort.fioDown:
                    return _employees = _employees.OrderByDescending(x => x.User.LastName).ToList();
                case Sort.DateOfBirthUp:
                    return _employees = _employees.OrderBy(x => x.User.DateOfBirth).ToList();
                case Sort.DateOfBirthDown:
                    return _employees = _employees.OrderByDescending(x => x.User.DateOfBirth).ToList();
                default:
                    throw new Exception("Не обнаружено перечисление сортировки");
            }
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
                    _sort = Sort.fioUp;
                    break;
                case 2:
                    _sort = Sort.fioDown;
                    break;
                case 3:
                    _sort = Sort.DateOfBirthUp;
                    break;
                case 4:
                    _sort = Sort.DateOfBirthDown;
                    break;
            }
            SortList();
            GenerateTable();
        }

        enum Sort
        {
            None,
            fioUp,
            fioDown,
            DateOfBirthUp,
            DateOfBirthDown

        }

        enum Filter
        {
            Genre,
            Post,

        }

        private void GenreFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenreFilterCb.SelectedItem == null)
                return;
            _genreFilterId = (GenreFilterCb.SelectedItem as Genre).Id;
            FilterList();
            GenerateTable();
        }

        private void PostFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PostFilterCb.SelectedItem == null)
                return;
            _postFilterId = (PostFilterCb.SelectedItem as Entities.Post).Id;
            FilterList();
            GenerateTable();

        }
        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            GenreFilterCb.SelectedItem = null;
            PostFilterCb.SelectedItem = null;
            _genreFilterId = 0;
            _postFilterId = 0;
            SearchTb.Text = "";
            LoadData();
        }
        private void GenerateTable()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _employees;
        }

        private async void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadData(SearchTb.Text);
        }
    }
}


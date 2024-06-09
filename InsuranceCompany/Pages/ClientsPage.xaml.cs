using Azure;
using InsuranceCompany.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {

        Sort _sort = Sort.None;
        private List<Client> _clients;
        private List<Client> _allClients;
        private int _genreFilterId;
        private int _groupFilterId;
        public ClientsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Entities.Context context = new();
            GenreFilterCb.ItemsSource = context.Genres.ToList();
            GroupFilterCb.ItemsSource = context.ClientGroups.ToList();
            _allClients = context.Clients
                .Include(x => x.Group)
                .Include(x => x.User)
                .ThenInclude(x => x.Genre)
                .Where(x => x.User.RoleId == 1 && x.User.IsActive == true)
                .ToList();
            _clients = _allClients;
            
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
            GenreFilterCb.ItemsSource = await context.Genres.ToListAsync();
            GroupFilterCb.ItemsSource = await context.ClientGroups.ToListAsync();
            _allClients = await context.Clients
                .Include(x => x.Group)
                .Include(x => x.User)
                .ThenInclude(x => x.Genre)
                .Where(x => x.User.RoleId == 1 && x.User.IsActive == true && (
                x.User.LastName.Contains(search) ||
                x.User.FirstName.Contains(search) ||
                x.User.Patronymic.Contains(search) ||
                x.User.Address.Contains(search) ||
                x.User.Telephone.Contains(search) ||
                x.User.Genre.Name.Contains(search) ||
                x.Group.Name.Contains(search) ||
                x.User.Passport.Contains(search)))
                .ToListAsync();
            _clients = _allClients;
            FilterList();
            GenerateTable();
        }

        private void GenerateTable()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _clients;
        }


        private void FilterList()
        {
            _clients = _allClients ;
            if (_genreFilterId != 0)
                _clients = _clients.Where(x => x.User.GenreId == _genreFilterId).ToList();
            if (_groupFilterId != 0)
                _clients = _clients.Where(x => x.GroupId == _groupFilterId).ToList();
            SortList();
        }

        private List<Entities.Client> SortList()
        {

            switch (_sort)
            {
                case Sort.None:
                    return _clients = _clients.OrderBy(x => x.Id).ToList();
                case Sort.fioUp:
                    return _clients = _clients.OrderBy(x => x.User.LastName).ToList();
                case Sort.fioDown:
                    return _clients = _clients.OrderByDescending(x => x.User.LastName).ToList();
                case Sort.DateOfBirthUp:
                    return _clients = _clients.OrderBy(x => x.User.DateOfBirth).ToList();
                case Sort.DateOfBirthDown:
                    return _clients = _clients.OrderByDescending(x => x.User.DateOfBirth).ToList();
                default:
                    throw new Exception("Нету сортировки");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditClientPage(null));

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditClientPage((sender as System.Windows.Controls.Button).DataContext as Client));
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
        private void GenreFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenreFilterCb.SelectedItem == null)
                return;
            _genreFilterId = (GenreFilterCb.SelectedItem as Genre).Id;
            FilterList();
            GenerateTable();
        }

        private void GroupFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupFilterCb.SelectedItem == null)
                return;
            _groupFilterId = (GroupFilterCb.SelectedItem as Entities.ClientGroup).Id;
            FilterList();
            GenerateTable();
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            GenreFilterCb.SelectedItem = null;
            GroupFilterCb.SelectedItem = null;
            _genreFilterId = 0;
            _groupFilterId = 0;
            SearchTb.Text = "";
            LoadData();
        }

        private async void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadData(SearchTb.Text);
        }
    }
}

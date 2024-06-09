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
    /// Логика взаимодействия для RiskPage.xaml
    /// </summary>
    public partial class RiskPage : Page
    {
        Sort _sort = Sort.None;
        private List<Risk> _risks;
        private List<Risk> _allRisks;
        public RiskPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Entities.Context context = new();
            _allRisks = context.Risks.ToList();
            _risks = _allRisks;
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
            _allRisks = await context.Risks
                .Where(x =>
                x.Name.Contains(search) ||
                x.Description.Contains(search) ||
                x.AverageProbability.ToString().Contains(search))
                .ToListAsync();
            _risks = _allRisks;
            GenerateTable();
        }

        private void GenerateTable()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = _risks;
        }

        private List<Entities.Risk> SortList()
        {

            switch (_sort)
            {
                case Sort.None:
                    return _risks = _risks.OrderBy(x => x.Id).ToList();
                case Sort.NameUp:
                    return _risks = _risks.OrderBy(x => x.Name).ToList();
                case Sort.NameDown:
                    return _risks = _risks.OrderByDescending(x => x.Name).ToList();
                default:
                    throw new Exception("Нету сортировки");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditRiskPage(null));

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainFrame.Navigate(new Pages.EditRiskPage((sender as System.Windows.Controls.Button).DataContext as Risk));
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

    }
}


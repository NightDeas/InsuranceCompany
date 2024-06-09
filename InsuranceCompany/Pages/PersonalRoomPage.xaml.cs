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
    /// Логика взаимодействия для PersonalRoomPage.xaml
    /// </summary>
    public partial class PersonalRoomPage : Page
    {
        public PersonalRoomPage()
        {
            InitializeComponent();
            LoadData();
            DataContext = App.User;
        }

        private void LoadData()
        {
            if (App.User.RoleId == 1)
            {
                PoliciesStackPanel.Visibility = Visibility.Visible;
                Entities.Context context = new();
                MainDataGrid.ItemsSource = context.Policies
                    .Include(x => x.Type)
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.User)
                    .Where(x => x.ClientId == App.User.Id)
                    .ToList();
            }
            else
            {
                PoliciesStackPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}

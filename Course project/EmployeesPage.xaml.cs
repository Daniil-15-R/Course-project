using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class EmployeesPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public EmployeesPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateEmployees();
        }

        private void UpdateEmployees()
        {
            var currentEmployees = Entities.GetContext().ShelterEmployees.ToList();

            if (!string.IsNullOrWhiteSpace(SearchEmployee.Text))
            {
                currentEmployees = currentEmployees.Where(x =>
                    x.FIO.ToLower().Contains(SearchEmployee.Text.ToLower())).ToList();
            }

            if (SortEmployee.SelectedIndex == 0)
            {
                currentEmployees = currentEmployees.OrderBy(x => x.FIO).ToList();
            }
            else if (SortEmployee.SelectedIndex == 1)
            {
                currentEmployees = currentEmployees.OrderByDescending(x => x.FIO).ToList();
            }
            else if (SortEmployee.SelectedIndex == 2)
            {
                currentEmployees = currentEmployees.OrderBy(x => x.date_of_birth).ToList();
            }
            else if (SortEmployee.SelectedIndex == 3)
            {
                currentEmployees = currentEmployees.OrderByDescending(x => x.date_of_birth).ToList();
            }

            ListViewEmployees.ItemsSource = currentEmployees;
        }

        private void SearchEmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEmployees();
        }

        private void SortEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEmployees();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchEmployee.Text = "";
            SortEmployee.SelectedIndex = -1;
            UpdateEmployees();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEmployees((sender as Button).DataContext as ShelterEmployees));
        }


        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var employeesForRemoving = ListViewEmployees.SelectedItems.Cast<ShelterEmployees>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {employeesForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ShelterEmployees.RemoveRange(employeesForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    new HomeScreen(_currentUser).Show();
                }
                else
                {
                    new HomeScreen2(_currentUser).Show();
                }
                Window.GetWindow(this)?.Close();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new MedicinePage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new VacinationPage(_currentUser, _userRole, true));
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new DogPage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new DogPage(_currentUser, _userRole, true));
            }
        }

        private void EmployeesPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateEmployees();
            }
        }
    }
}
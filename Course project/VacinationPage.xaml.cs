using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class VacinationPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public VacinationPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateVaccinations();
        }

        private void UpdateVaccinations()
        {
            var currentVaccinations = Entities.GetContext().VaccinationSchedule.ToList();

            if (!string.IsNullOrWhiteSpace(SearchVacination.Text))
            {
                currentVaccinations = currentVaccinations.Where(x =>
                    x.title.ToLower().Contains(SearchVacination.Text.ToLower())).ToList();
            }

            if (SortVacination.SelectedIndex == 0)
            {
                currentVaccinations = currentVaccinations.OrderBy(x => x.title).ToList();
            }
            else if (SortVacination.SelectedIndex == 1)
            {
                currentVaccinations = currentVaccinations.OrderByDescending(x => x.title).ToList();
            }
            else if (SortVacination.SelectedIndex == 2)
            {
                currentVaccinations = currentVaccinations.OrderByDescending(x => x.date).ToList();
            }
            else if (SortVacination.SelectedIndex == 3)
            {
                currentVaccinations = currentVaccinations.OrderBy(x => x.date).ToList();
            }

            DataGridVac.ItemsSource = currentVaccinations;
        }

        private void SearchVacination_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateVaccinations();
        }

        private void SortVacination_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateVaccinations();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchVacination.Text = "";
            SortVacination.SelectedIndex = -1;
            UpdateVaccinations();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageVac((sender as Button).DataContext as VaccinationSchedule));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageVac(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var vaccinationsForRemoving = DataGridVac.SelectedItems.Cast<VaccinationSchedule>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {vaccinationsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().VaccinationSchedule.RemoveRange(vaccinationsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateVaccinations();
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
            if (_fromHomeScreen2)
            {
                ParasitePage parasitePage = new ParasitePage(_currentUser, _userRole, true);
                NavigationService.Navigate(parasitePage);
            }
            else
            {
                FinancePage financePage = new FinancePage(_currentUser, _userRole);
                NavigationService.Navigate(financePage);
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fromHomeScreen2)
            {
                EmployeesPage employeesPage = new EmployeesPage(_currentUser, _userRole, true);
                NavigationService.Navigate(employeesPage);
            }
            else
            {
                MedicinePage medicinePage = new MedicinePage(_currentUser, _userRole);
                NavigationService.Navigate(medicinePage);
            }
        }

        private void VacinPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateVaccinations();
            }
        }
    }
}
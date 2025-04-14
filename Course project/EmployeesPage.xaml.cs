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

        public EmployeesPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateEmployees();
        }

        private void UpdateEmployees()
        {
            var currentEmployees = Entities.GetContext().ShelterEmployees.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchEmployee.Text))
            {
                currentEmployees = currentEmployees.Where(x =>
                    x.FIO.ToLower().Contains(SearchEmployee.Text.ToLower())).ToList();
            }

            // Сортировка
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

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEmployees(null));
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
                HomeScreen homeScreen = new HomeScreen(_currentUser);
                homeScreen.Show();
                Window.GetWindow(this)?.Close();
            }
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            MedicinePage medicinePage = new MedicinePage(_currentUser, _userRole);
            NavigationService.Navigate(medicinePage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            DogPage dogPage = new DogPage(_currentUser, _userRole);
            NavigationService.Navigate(dogPage);
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
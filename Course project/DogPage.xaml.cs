using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class DogPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public DogPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateDogs();
        }

        private void UpdateDogs()
        {
            var currentDog = Entities.GetContext().Dogs.ToList();

            if (!string.IsNullOrWhiteSpace(SearchDog.Text))
            {
                currentDog = currentDog.Where(x =>
                    x.nickname.ToLower().Contains(SearchDog.Text.ToLower())).ToList();
            }

            if (SortAge.SelectedIndex == 0)
            {
                currentDog = currentDog.OrderBy(x => x.age).ToList();
            }
            else if (SortAge.SelectedIndex == 1)
            {
                currentDog = currentDog.OrderByDescending(x => x.age).ToList();
            }

            ListViewDogs.ItemsSource = currentDog;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageDog((sender as Button).DataContext as Dogs));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageDog(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var dogsForRemoving = ListViewDogs.SelectedItems.Cast<Dogs>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {dogsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Dogs.RemoveRange(dogsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateDogs();
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
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new MedicinePage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new EmployeesPage(_currentUser, _userRole, true));
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new FinancePage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new EventPage(_currentUser, _userRole, true));
            }
        }

        private void DogPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateDogs();
            }
        }

        private void SearchDog_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDogs();
        }

        private void SortAge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDogs();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchDog.Text = "";
            SortAge.SelectedIndex = -1;
            UpdateDogs();
        }
    }
}
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class WalkingPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public WalkingPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateWalks();
        }

        private void UpdateWalks()
        {
            var currentWalks = Entities.GetContext().WalkingSchedule.ToList();

            if (!string.IsNullOrWhiteSpace(SearchWalk.Text))
            {
                currentWalks = currentWalks.Where(x =>
                    x.FIO.ToLower().Contains(SearchWalk.Text.ToLower())).ToList();
            }

            if (SortWalk.SelectedIndex == 0)
            {
                currentWalks = currentWalks.OrderBy(x => x.FIO).ToList();
            }
            else if (SortWalk.SelectedIndex == 1)
            {
                currentWalks = currentWalks.OrderByDescending(x => x.FIO).ToList();
            }
            else if (SortWalk.SelectedIndex == 2)
            {
                currentWalks = currentWalks.OrderBy(x => x.time).ToList();
            }
            else if (SortWalk.SelectedIndex == 3)
            {
                currentWalks = currentWalks.OrderByDescending(x => x.time).ToList();
            }

            DataGridWalk.ItemsSource = currentWalks;
        }

        private void SearchWalk_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWalks();
        }

        private void SortWalk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWalks();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchWalk.Text = "";
            SortWalk.SelectedIndex = -1;
            UpdateWalks();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageWalking((sender as Button).DataContext as WalkingSchedule));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageWalking(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var walksForRemoving = DataGridWalk.SelectedItems.Cast<WalkingSchedule>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {walksForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().WalkingSchedule.RemoveRange(walksForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateWalks();
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
                NavigationService.Navigate(new DogPage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new EventPage(_currentUser, _userRole, true));
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new VacinationPage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new ParasitePage(_currentUser, _userRole, true));
            }
        }

        private void WalkPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateWalks();
            }
        }
    }
}
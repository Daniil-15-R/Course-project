using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class EventPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public EventPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateEvents();
        }

        private void UpdateEvents()
        {
            var currentEvents = Entities.GetContext().PlannedEvents.ToList();

            if (!string.IsNullOrWhiteSpace(SearchEvent.Text))
            {
                currentEvents = currentEvents.Where(x =>
                    x.title.ToLower().Contains(SearchEvent.Text.ToLower())).ToList();
            }

            if (SortEvent.SelectedIndex == 0)
            {
                currentEvents = currentEvents.OrderBy(x => x.title).ToList();
            }
            else if (SortEvent.SelectedIndex == 1)
            {
                currentEvents = currentEvents.OrderByDescending(x => x.title).ToList();
            }
            else if (SortEvent.SelectedIndex == 2)
            {
                currentEvents = currentEvents.OrderByDescending(x => x.date).ToList();
            }
            else if (SortEvent.SelectedIndex == 3)
            {
                currentEvents = currentEvents.OrderBy(x => x.date).ToList();
            }

            DataGridEvent.ItemsSource = currentEvents;
        }

        private void SearchEvent_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEvents();
        }

        private void SortEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEvents();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchEvent.Text = "";
            SortEvent.SelectedIndex = -1;
            UpdateEvents();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEvent((sender as Button).DataContext as PlannedEvents));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEvent(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var eventsForRemoving = DataGridEvent.SelectedItems.Cast<PlannedEvents>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {eventsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().PlannedEvents.RemoveRange(eventsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateEvents();
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
                DogPage dogPage = new DogPage(_currentUser, _userRole, true);
                NavigationService.Navigate(dogPage);
            }
            else
            {
                // Укажите страницу для перехода из HomeScreen (если нужно)
                EmployeesPage employeesPage = new EmployeesPage(_currentUser, _userRole);
                NavigationService.Navigate(employeesPage);
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fromHomeScreen2)
            {
                WalkingPage walkingPage = new WalkingPage(_currentUser, _userRole, true);
                NavigationService.Navigate(walkingPage);
            }
            else
            {
                // Укажите страницу для перехода из HomeScreen (если нужно)
                ParasitePage parasitePage = new ParasitePage(_currentUser, _userRole);
                NavigationService.Navigate(parasitePage);
            }
        }

        private void EventPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateEvents();
            }
        }
    }
}
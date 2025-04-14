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

        public EventPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateEvents();
        }

        private void UpdateEvents()
        {
            var currentEvents = Entities.GetContext().PlannedEvents.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchEvent.Text))
            {
                currentEvents = currentEvents.Where(x =>
                    x.title.ToLower().Contains(SearchEvent.Text.ToLower())).ToList();
            }

            // Сортировка
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
                HomeScreen homeScreen = new HomeScreen(_currentUser);
                homeScreen.Show();
                Window.GetWindow(this)?.Close();
            }
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            DogPage dogPage = new DogPage(_currentUser, _userRole);
            NavigationService.Navigate(dogPage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            WalkingPage walkingPage = new WalkingPage(_currentUser, _userRole);
            NavigationService.Navigate(walkingPage);
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
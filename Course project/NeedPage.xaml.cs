using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class NeedPage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public NeedPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateNeeds();
        }

        private void UpdateNeeds()
        {
            var currentNeeds = Entities.GetContext().ShelterNeeds.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchNeed.Text))
            {
                currentNeeds = currentNeeds.Where(x =>
                    x.title.ToLower().Contains(SearchNeed.Text.ToLower())).ToList();
            }

            // Сортировка
            if (SortNeed.SelectedIndex == 0)
            {
                currentNeeds = currentNeeds.OrderBy(x => x.title).ToList();
            }
            else if (SortNeed.SelectedIndex == 1)
            {
                currentNeeds = currentNeeds.OrderByDescending(x => x.title).ToList();
            }
            else if (SortNeed.SelectedIndex == 2)
            {
                currentNeeds = currentNeeds.OrderBy(x => x.cost).ToList();
            }
            else if (SortNeed.SelectedIndex == 3)
            {
                currentNeeds = currentNeeds.OrderByDescending(x => x.cost).ToList();
            }

            DataGridNeeds.ItemsSource = currentNeeds;
        }

        private void SearchNeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateNeeds();
        }

        private void SortNeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateNeeds();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchNeed.Text = "";
            SortNeed.SelectedIndex = -1;
            UpdateNeeds();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageNeeds((sender as Button).DataContext as ShelterNeeds));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageNeeds(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var needsForRemoving = DataGridNeeds.SelectedItems.Cast<ShelterNeeds>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {needsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ShelterNeeds.RemoveRange(needsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateNeeds();
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
            FinancePage financePage = new FinancePage(_currentUser, _userRole);
            NavigationService.Navigate(financePage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            FoodPage foodPage = new FoodPage(_currentUser, _userRole);
            NavigationService.Navigate(foodPage);
        }

        private void NeedPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateNeeds();
            }
        }
    }
}
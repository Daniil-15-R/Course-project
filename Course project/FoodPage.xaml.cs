using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class FoodPage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public FoodPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateFoodProducts();
        }

        private void UpdateFoodProducts()
        {
            var currentFood = Entities.GetContext().FoodProducts.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchFood.Text))
            {
                currentFood = currentFood.Where(x =>
                    x.name_of_food.ToLower().Contains(SearchFood.Text.ToLower())).ToList();
            }

            // Сортировка
            if (SortFood.SelectedIndex == 0)
            {
                currentFood = currentFood.OrderBy(x => x.name_of_food).ToList();
            }
            else if (SortFood.SelectedIndex == 1)
            {
                currentFood = currentFood.OrderByDescending(x => x.name_of_food).ToList();
            }
            else if (SortFood.SelectedIndex == 2)
            {
                currentFood = currentFood.OrderBy(x => x.cost).ToList();
            }
            else if (SortFood.SelectedIndex == 3)
            {
                currentFood = currentFood.OrderByDescending(x => x.cost).ToList();
            }

            DataGridFood.ItemsSource = currentFood;
        }

        private void SearchFood_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFoodProducts();
        }

        private void SortFood_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFoodProducts();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchFood.Text = "";
            SortFood.SelectedIndex = -1;
            UpdateFoodProducts();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFood((sender as Button).DataContext as FoodProducts));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFood(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var foodForRemoving = DataGridFood.SelectedItems.Cast<FoodProducts>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {foodForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().FoodProducts.RemoveRange(foodForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateFoodProducts();
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
            NeedPage needPage = new NeedPage(_currentUser, _userRole);
            NavigationService.Navigate(needPage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            MedicinePage medicinePage = new MedicinePage(_currentUser, _userRole);
            NavigationService.Navigate(medicinePage);
        }

        private void FoodPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateFoodProducts();
            }
        }
    }
}
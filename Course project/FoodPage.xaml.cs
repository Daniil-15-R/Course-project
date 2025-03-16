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

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для FoodPage.xaml
    /// </summary>
    public partial class FoodPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public FoodPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var dogsList = Entities.GetContext().FoodProducts.ToList();
            DataGridFood.ItemsSource = dogsList;
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

                    DataGridFood.ItemsSource = Entities.GetContext().FoodProducts.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли предыдущие страницы в стеке навигации
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                // Если нет страниц для возврата, переходим на HomeScreen
                HomeScreen homeScreen = new HomeScreen(_currentUser);
                homeScreen.Show();
                Window.GetWindow(this)?.Close();
            }
        }
        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            MedicinePage medicinePage = new MedicinePage(_currentUser, _userRole);
            NavigationService.Navigate(medicinePage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            NeedPage needPage = new NeedPage(_currentUser, _userRole);
            NavigationService.Navigate(needPage);
        }
        private void FoodPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridFood.ItemsSource = Entities.GetContext().FoodProducts.ToList();
            }
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для DogPage.xaml
    /// </summary>
    public partial class DogPage : Page
    {
        private Users _currentUser;
        private string _userRole;

        // Конструктор, принимающий Users и роль
        public DogPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var dogsList = Entities.GetContext().Dogs.ToList();
            ListViewDogs.ItemsSource = dogsList;
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

                    ListViewDogs.ItemsSource = Entities.GetContext().Dogs.ToList();
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

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            EmployeesPage employeesPage = new EmployeesPage(_currentUser, _userRole);
            NavigationService.Navigate(employeesPage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            EventPage eventPage = new EventPage(_currentUser, _userRole);
            NavigationService.Navigate(eventPage);
        }

        private void DogPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                ListViewDogs.ItemsSource = Entities.GetContext().Dogs.ToList();
            }
        }
    }
}
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
            MessageBox.Show("Конструктор DogPage выполнен.");

            var dogsList = Entities1.GetContext().Dogs.ToList();
            DataGridDogs.ItemsSource = dogsList;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования выбранной собаки
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            // Навигация на страницу для добавления новой собаки

        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            // Логика для удаления выбранной собаки
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
    }
}
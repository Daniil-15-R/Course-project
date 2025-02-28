using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class MedicinePage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public MedicinePage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли предыдущие страницы в стеке навигации
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                // Если есть, то возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            else
            {
                // Если NavigationService отсутствует или нет страниц для возврата, 
                // то переходим на HomeScreen
                // Проверяем, что окно не закрывается, а открывается новое окно для HomeScreen
                HomeScreen homeScreen = new HomeScreen(_currentUser);
                homeScreen.Show();
                // Закрываем текущее окно (если это необходимо)
                Window.GetWindow(this)?.Close();
            }
        }
    }
}
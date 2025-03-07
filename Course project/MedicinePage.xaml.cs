using System.Linq;
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

            var medicinesList = Entities1.GetContext().Medicines.ToList();
            DataGridMedicines.ItemsSource = medicinesList;
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
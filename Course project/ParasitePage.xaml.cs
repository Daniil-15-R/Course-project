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
    /// Логика взаимодействия для ParasitePage.xaml
    /// </summary>
    public partial class ParasitePage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public ParasitePage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var parasiteList = Entities1.GetContext().ParasiteTreatmentSchedule.ToList();
            DataGridParasite.ItemsSource = parasiteList;
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
        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            VacinationPage vacPage = new VacinationPage(_currentUser, _userRole);
            NavigationService.Navigate(vacPage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            WalkingPage walkingPage = new WalkingPage(_currentUser, _userRole);
            NavigationService.Navigate(walkingPage);
        }
    }
}

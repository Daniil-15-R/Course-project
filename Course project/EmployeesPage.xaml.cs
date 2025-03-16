using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public EmployeesPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var employeesList = Entities.GetContext().ShelterEmployees.ToList();
            ListViewEmployees.ItemsSource = employeesList;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEmployees((sender as Button).DataContext as ShelterEmployees));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var employeesForRemoving = ListViewEmployees.SelectedItems.Cast<ShelterEmployees>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {employeesForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ShelterEmployees.RemoveRange(employeesForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    ListViewEmployees.ItemsSource = Entities.GetContext().ShelterEmployees.ToList();
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
            DogPage dogPage = new DogPage(_currentUser, _userRole);
            NavigationService.Navigate(dogPage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            MedicinePage medicinePage = new MedicinePage(_currentUser, _userRole);
            NavigationService.Navigate(medicinePage);
        }
        private void EmployeesPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                ListViewEmployees.ItemsSource = Entities.GetContext().ShelterEmployees.ToList();
            }
        }
    }
}
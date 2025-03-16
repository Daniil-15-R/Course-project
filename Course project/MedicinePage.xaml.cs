using System;
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

            var medicinesList = Entities.GetContext().Medicines.ToList();
            DataGridMedicines.ItemsSource = medicinesList;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddMedicinePage((sender as Button).DataContext as Medicines));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddMedicinePage(null));

        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var medicinesForRemoving = DataGridMedicines.SelectedItems.Cast<Medicines>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {medicinesForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Medicines.RemoveRange(medicinesForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridMedicines.ItemsSource = Entities.GetContext().Medicines.ToList();
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
            EmployeesPage employeesPage = new EmployeesPage(_currentUser, _userRole);
            NavigationService.Navigate(employeesPage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            FoodPage foodPage = new FoodPage(_currentUser, _userRole);
            NavigationService.Navigate(foodPage);
        }
        private void MedicinePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridMedicines.ItemsSource = Entities.GetContext().Medicines.ToList();
            }
        }
    }
}
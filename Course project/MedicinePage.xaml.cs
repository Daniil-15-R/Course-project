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
            _userRole = userRole;

            UpdateMedicines();
        }

        private void UpdateMedicines()
        {
            var currentMedicines = Entities.GetContext().Medicines.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchMedicine.Text))
            {
                currentMedicines = currentMedicines.Where(x =>
                    x.name_of_medicine.ToLower().Contains(SearchMedicine.Text.ToLower())).ToList();
            }

            // Сортировка
            if (SortMedicine.SelectedIndex == 0)
            {
                currentMedicines = currentMedicines.OrderBy(x => x.name_of_medicine).ToList();
            }
            else if (SortMedicine.SelectedIndex == 1)
            {
                currentMedicines = currentMedicines.OrderByDescending(x => x.name_of_medicine).ToList();
            }
            else if (SortMedicine.SelectedIndex == 2)
            {
                currentMedicines = currentMedicines.OrderBy(x => x.cost).ToList();
            }
            else if (SortMedicine.SelectedIndex == 3)
            {
                currentMedicines = currentMedicines.OrderByDescending(x => x.cost).ToList();
            }

            DataGridMedicines.ItemsSource = currentMedicines;
        }

        private void SearchMedicine_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMedicines();
        }

        private void SortMedicine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMedicines();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchMedicine.Text = "";
            SortMedicine.SelectedIndex = -1;
            UpdateMedicines();
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
                    UpdateMedicines();
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
            FoodPage foodPage = new FoodPage(_currentUser, _userRole);
            NavigationService.Navigate(foodPage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            EmployeesPage employeesPage = new EmployeesPage(_currentUser, _userRole);
            NavigationService.Navigate(employeesPage);
        }

        private void MedicinePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateMedicines();
            }
        }
    }
}
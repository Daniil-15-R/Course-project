using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class FinancePage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public FinancePage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateFinances();
        }

        private void UpdateFinances()
        {
            var currentFinances = Entities.GetContext().Accounting.ToList();

            // Фильтрация по поиску (по дате)
            if (!string.IsNullOrWhiteSpace(SearchFinance.Text))
            {
                currentFinances = currentFinances.Where(x =>
                    x.accounting_date.ToString().Contains(SearchFinance.Text)).ToList();
            }

            // Сортировка
            if (SortFinance.SelectedIndex == 0)
            {
                currentFinances = currentFinances.OrderByDescending(x => x.accounting_date).ToList();
            }
            else if (SortFinance.SelectedIndex == 1)
            {
                currentFinances = currentFinances.OrderBy(x => x.accounting_date).ToList();
            }
            else if (SortFinance.SelectedIndex == 2)
            {
                currentFinances = currentFinances.OrderBy(x => x.total).ToList();
            }
            else if (SortFinance.SelectedIndex == 3)
            {
                currentFinances = currentFinances.OrderByDescending(x => x.total).ToList();
            }

            DataGridFinance.ItemsSource = currentFinances;
        }

        private void SearchFinance_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFinances();
        }

        private void SortFinance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFinances();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchFinance.Text = "";
            SortFinance.SelectedIndex = -1;
            UpdateFinances();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFinance((sender as Button).DataContext as Accounting));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFinance(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var financesForRemoving = DataGridFinance.SelectedItems.Cast<Accounting>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {financesForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Accounting.RemoveRange(financesForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateFinances();
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
            VacinationPage vacPage = new VacinationPage(_currentUser, _userRole);
            NavigationService.Navigate(vacPage);
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            NeedPage needPage = new NeedPage(_currentUser, _userRole);
            NavigationService.Navigate(needPage);
        }

        private void FinancePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateFinances();
            }
        }
    }
}
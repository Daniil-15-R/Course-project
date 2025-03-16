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
    /// Логика взаимодействия для FinancePage.xaml
    /// </summary>
    public partial class FinancePage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public FinancePage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var financeList = Entities.GetContext().Accounting.ToList();
            DataGridFinance.ItemsSource = financeList;
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
            var financeForRemoving = DataGridFinance.SelectedItems.Cast<Dogs>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {financeForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Dogs.RemoveRange(financeForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridFinance.ItemsSource = Entities.GetContext().Accounting.ToList();
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
            NeedPage needPage = new NeedPage(_currentUser, _userRole);
            NavigationService.Navigate(needPage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            VacinationPage vacPage = new VacinationPage(_currentUser, _userRole);
            NavigationService.Navigate(vacPage);
        }
        private void FinancePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridFinance.ItemsSource = Entities.GetContext().Accounting.ToList();
            }
        }
    }
}

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
    /// Логика взаимодействия для VacinationPage.xaml
    /// </summary>
    public partial class VacinationPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public VacinationPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var vacList = Entities.GetContext().VaccinationSchedule.ToList();
            DataGridVac.ItemsSource = vacList;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageVac((sender as Button).DataContext as VaccinationSchedule));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageVac(null));

        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var vacForRemoving = DataGridVac.SelectedItems.Cast<VaccinationSchedule>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {vacForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().VaccinationSchedule.RemoveRange(vacForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridVac.ItemsSource = Entities.GetContext().VaccinationSchedule.ToList();
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
            FinancePage financePage = new FinancePage(_currentUser, _userRole);
            NavigationService.Navigate(financePage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            ParasitePage parasitePage = new ParasitePage(_currentUser, _userRole);
            NavigationService.Navigate(parasitePage);
        }
        private void VacinPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridVac.ItemsSource = Entities.GetContext().VaccinationSchedule.ToList();
            }
        }
    }
}

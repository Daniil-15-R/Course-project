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
    /// Логика взаимодействия для WalkingPage.xaml
    /// </summary>
    public partial class WalkingPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        public WalkingPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole; // Сохранение роли

            var walkList = Entities.GetContext().WalkingSchedule.ToList();
            DataGridWalk.ItemsSource = walkList;
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageWalking((sender as Button).DataContext as WalkingSchedule));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageWalking(null));

        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var walkForRemoving = DataGridWalk.SelectedItems.Cast<WalkingSchedule>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {walkForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().WalkingSchedule.RemoveRange(walkForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridWalk.ItemsSource = Entities.GetContext().WalkingSchedule.ToList();
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
            ParasitePage parasitePage = new ParasitePage(_currentUser, _userRole);
            NavigationService.Navigate(parasitePage);
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            EventPage eventPage = new EventPage(_currentUser, _userRole);
            NavigationService.Navigate(eventPage);
        }
        private void WalkPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridWalk.ItemsSource = Entities.GetContext().VaccinationSchedule.ToList();
            }
        }
    }
}

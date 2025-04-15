using System;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class HomeScreen2 : Window
    {
        private Users _currentUser;

        public HomeScreen2(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void NavigateToPage(Page page)
        {
            // Скрываем и деактивируем элементы управления
            SetButtonsVisibility(false);
            SetTextBlocksVisibility(false);

            // Переходим на страницу
            MainFrame.Navigate(page);
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            DogPage dogPage = new DogPage(_currentUser, _currentUser.role);
            NavigateToPage(dogPage);
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesPage employeesPage = new EmployeesPage(_currentUser, _currentUser.role);
            NavigateToPage(employeesPage);
        }

        private void VacinButton_Click(object sender, RoutedEventArgs e)
        {
            VacinationPage vacinatPage = new VacinationPage(_currentUser, _currentUser.role);
            NavigateToPage(vacinatPage);
        }

        private void ParasiteButton_Click(object sender, RoutedEventArgs e)
        {
            ParasitePage parasitePage = new ParasitePage(_currentUser, _currentUser.role);
            NavigateToPage(parasitePage);
        }

        private void WalkingButton_Click(object sender, RoutedEventArgs e)
        {
            WalkingPage walkingPage = new WalkingPage(_currentUser, _currentUser.role);
            NavigateToPage(walkingPage);
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            EventPage eventPage = new EventPage(_currentUser, _currentUser.role);
            NavigateToPage(eventPage);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SetButtonsVisibility(bool isVisible)
        {
            DogButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EmployeesButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            VacinButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            ParasiteButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            WalkingButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EventButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            BackButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

            DogButton.IsEnabled = isVisible;
            EmployeesButton.IsEnabled = isVisible;
            VacinButton.IsEnabled = isVisible;
            ParasiteButton.IsEnabled = isVisible;
            WalkingButton.IsEnabled = isVisible;
            EventButton.IsEnabled = isVisible;
            BackButton.IsEnabled = isVisible;
        }

        private void SetTextBlocksVisibility(bool isVisible)
        {
            DogBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EmployeesBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            VacinBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            ParasiteBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            WalkingBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EventBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Если Frame пуст (вернулись на главный экран)
            if (e.Content == null || e.Content == this)
            {
                SetButtonsVisibility(true);
                SetTextBlocksVisibility(true);
            }
            else // Если перешли на другую страницу
            {
                SetButtonsVisibility(false);
                SetTextBlocksVisibility(false);
            }
        }
    }
}
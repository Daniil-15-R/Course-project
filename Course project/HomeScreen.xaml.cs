using System;
using System.Windows;
using System.Windows.Navigation;

namespace Course_project
{
    public partial class HomeScreen : Window
    {
        private Users _currentUser; // Поле для хранения текущего пользователя

        public HomeScreen(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser; // Сохранение пользователя

            // Подписываемся на события навигации
            this.MainFrame.Navigated += MainFrame_Navigated;
            this.MainFrame.Navigating += MainFrame_Navigating;
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            // Блокировка кнопок при навигации на новую страницу
            BlockButtons();
            BlockBlock();

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Возвращение кнопок в активное состояние, если вернулись на HomeScreen
            if (e.Content is HomeScreen)
            {
                ActivateButtons();
            }
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            DogPage dogPage = new DogPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(dogPage);
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            EmployeesPage employeesPage = new EmployeesPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(employeesPage);
        }

        private void MedicineButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            MedicinePage medicinePage = new MedicinePage (_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(medicinePage);
        }

        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            FoodPage foodPage = new FoodPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(foodPage);
        }

        private void NeedsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            NeedPage needPage = new NeedPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(needPage);
        }

        private void FinanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            FinancePage financePage = new FinancePage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(financePage);
        }

        private void VacinButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            VacinationPage vacinationPage = new VacinationPage (_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(vacinationPage);
        }

        private void ParasiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            ParasitePage parasitePage = new ParasitePage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(parasitePage);
        }

        private void WalkingButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            WalkingPage walkingPage = new WalkingPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(walkingPage);
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Пользователь не инициализирован.");
                return;
            }

            EventPage eventPage = new EventPage(_currentUser, App.Current.Properties["UserRole"]?.ToString() ?? "Гость");
            MainFrame.Navigate(eventPage);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Переход обратно на главное окно
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BlockButtons()
        {
            // Блокировка и скрытие всех кнопок
            SetButtonsVisibility(false);
        }

        private void ActivateButtons()
        {
            // Активация и показ всех кнопок
            SetButtonsVisibility(true);
        }
        private void BlockBlock()
        {
            SetTextBlocksVisibility(false);
        }

        private void SetButtonsVisibility(bool isVisible)
        {
            // Установка видимости всех кнопок
            DogButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EmployeesButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            MedicineButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            FoodButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            NeedsButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            FinanceButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            VacinButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            ParasiteButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            WalkingButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EventButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            BackButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;

            // Дополнительно можно блокировать кнопки
            foreach (var button in new[] { DogButton, EmployeesButton, MedicineButton, FoodButton,
                                             NeedsButton, FinanceButton, VacinButton,
                                             ParasiteButton, WalkingButton, EventButton,
                                             BackButton })
            {
                button.IsEnabled = isVisible;
            }
        }
            private void SetTextBlocksVisibility(bool isVisible)
        {
            DogBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EmployeesBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            MedicineBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            FoodBlock.Visibility = isVisible ? Visibility.Visible: Visibility.Collapsed;
            NeedsBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            FinanceBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            VacinBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            ParasiteBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            WalkingBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            EventBlock.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
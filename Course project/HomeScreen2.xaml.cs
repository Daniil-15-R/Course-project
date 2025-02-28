using System;
using System.Windows;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для HomeScreen2.xaml
    /// </summary>
    public partial class HomeScreen2 : Window
    {
        private Users _currentUser; // Поле для хранения текущего пользователя

        public HomeScreen2(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser; // Сохранение пользователя
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
           /* DogScreen dogScreen = new DogScreen();
            dogScreen.Show();
            this.Close();*/
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            /*EmployeesScreen employeesScreen = new EmployeesScreen();
            employeesScreen.Show();
            this.Close();*/
        }

        private void VacinButton_Click(object sender, RoutedEventArgs e)
        {
            /*VacinationScreen vacinationScreen = new VacinationScreen();
            vacinationScreen.Show();
            this.Close();*/
        }

        private void ParasiteButton_Click(object sender, RoutedEventArgs e)
        {
            /*ParasiteScreen parasiteScreen = new ParasiteScreen();
            parasiteScreen.Show();
            this.Close();*/
        }

        private void WalkingButton_Click(object sender, RoutedEventArgs e)
        {
            /*WalkingScreen walkingScreen = new WalkingScreen();
            walkingScreen.Show();
            this.Close();*/
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            /*EventScreen eventScreen = new EventScreen();
            eventScreen.Show();
            this.Close();*/
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
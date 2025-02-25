using System;
using System.Windows;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        private Users _currentUser; // Поле для хранения текущего пользователя

        public HomeScreen(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser; // Сохранение пользователя
        }

        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            DogScreen dogScreen = new DogScreen();
            dogScreen.Show();
            this.Close(); // Закрывает текущее окно
        }

        /*private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesScreen employeesScreen = new EmployeesScreen();
            employeesScreen.Show();
            this.Close();
        }

        private void MedicineButton_Click(object sender, RoutedEventArgs e)
        {
            MedicineScreen medicineScreen = new MedicineScreen();
            medicineScreen.Show();
            this.Close();
        }

        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            FoodScreen foodScreen = new FoodScreen();
            foodScreen.Show();
            this.Close();
        }

        private void NeedsButton_Click(object sender, RoutedEventArgs e)
        {
            NeedScreen needScreen = new NeedScreen();
            needScreen.Show();
            this.Close();
        }

        private void FinanceButton_Click(object sender, RoutedEventArgs e)
        {
            FinanceScreen financeScreen = new FinanceScreen();
            financeScreen.Show();
            this.Close();
        }

        private void VacinButton_Click(object sender, RoutedEventArgs e)
        {
            VacinationScreen vacinationScreen = new VacinationScreen();
            vacinationScreen.Show();
            this.Close();
        }

        private void ParasiteButton_Click(object sender, RoutedEventArgs e)
        {
            ParasiteScreen parasiteScreen = new ParasiteScreen();
            parasiteScreen.Show();
            this.Close();
        }

        private void WalkingButton_Click(object sender, RoutedEventArgs e)
        {
            WalkingScreen walkingScreen = new WalkingScreen();
            walkingScreen.Show();
            this.Close();
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            EventScreen eventScreen = new EventScreen();
            eventScreen.Show();
            this.Close();
        }*/

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
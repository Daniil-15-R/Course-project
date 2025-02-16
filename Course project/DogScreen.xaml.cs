using System;
using System.Linq;
using System.Windows;

namespace Course_project
{
    public partial class DogScreen : Window
    {
        public DogScreen()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = GetCurrentUser(); // Получение текущего пользователя

            if (currentUser != null)
            {
                string userRole = currentUser.role; // Получение роли текущего пользователя

                // Navigate based on user role
                if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    HomeScreen homeScreen = new HomeScreen(currentUser);
                    homeScreen.Show();
                }
                else // For all other roles
                {
                    HomeScreen2 homeScreen2 = new HomeScreen2(currentUser);
                    homeScreen2.Show();
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден.");
            }

            this.Close(); // Закрыть текущее окно
        }

        private Users GetCurrentUser()
        {
            string username = App.Current.Properties["Username"]?.ToString();
            string password = App.Current.Properties["Password"]?.ToString();

            using (var context = new Entities1())
            {
                return context.Users
                              .FirstOrDefault(u => u.Login == username && u.password == password);
            }
        }
    }
}
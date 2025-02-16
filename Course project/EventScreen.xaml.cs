using System.Linq;
using System.Windows;

namespace Course_project
{
    public partial class EventScreen : Window
    {
        public EventScreen()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.Properties["UserRole"] != null)
            {
                string userRole = App.Current.Properties["UserRole"].ToString();
                var currentUser = GetCurrentUser(); // Получение текущего пользователя

                if (currentUser != null)
                {
                    if (userRole == "admin")
                    {
                        HomeScreen homeScreen = new HomeScreen(currentUser);
                        homeScreen.Show();
                    }
                    else if (userRole == "user" || userRole == "Employee" || userRole == "Volunteer")
                    {
                        HomeScreen2 homeScreen2 = new HomeScreen2(currentUser);
                        homeScreen2.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
            this.Close(); // Закрыть текущее окно
        }

        private Users GetCurrentUser()
        {
            string username = App.Current.Properties["Username"]?.ToString(); // Предположим, что имя пользователя сохранено в свойствах приложения
            string password = App.Current.Properties["Password"]?.ToString(); // Предположим, что пароль сохранен в свойствах приложения

            using (var context = new Entities1())
            {
                // Предположим, что у вас есть класс User с полями Username и Password
                return context.Users
                              .FirstOrDefault(u => u.Login == username && u.password == password);
            }
        }
    }
}
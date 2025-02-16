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
using System.Windows.Shapes;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для VacinationScreen.xaml
    /// </summary>
    public partial class VacinationScreen : Window
    {
        public VacinationScreen()
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

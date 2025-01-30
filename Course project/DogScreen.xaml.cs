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
            // Проверяем, какую роль пользователя мы сохранили
            if (App.Current.Properties["UserRole"] != null)
            {
                string userRole = App.Current.Properties["UserRole"].ToString();
                if (userRole == "admin")
                {
                    HomeScreen homeScreen = new HomeScreen();
                    homeScreen.Show();
                }
                else if (userRole == "user")
                {
                    HomeScreen2 homeScreen2 = new HomeScreen2();
                    homeScreen2.Show();
                }
            }
            this.Close(); // Закрыть текущее окно
        }
    }
}

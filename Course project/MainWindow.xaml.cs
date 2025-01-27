using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedLanguage = selectedItem.Content.ToString();
                switch (selectedLanguage)
                {
                    case "Русский":
                        TextBlockTitle.Text = "Авторизация";
                        TextBlockLogin.Text = "Логин:";
                        TextBlockPassword.Text = "Пароль:";
                        TextBlocklanguage.Text = "Выберите язык";
                        break;
                    case "English":
                        TextBlockTitle.Text = "Authorization";
                        TextBlockLogin.Text = "Login:";
                        TextBlockPassword.Text = "Password:";
                        TextBlocklanguage.Text = "Select a language";
                        break;
                    case "Español":
                        TextBlockTitle.Text = "Autorización";
                        TextBlockLogin.Text = "Usuario:";
                        TextBlockPassword.Text = "Contraseña:";
                        TextBlocklanguage.Text = "Seleccione un idioma";
                        break;
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка логина и пароля
            if (LoginTextBox.Text == "admin" && PasswordTextBox.Password == "admin") // Используйте PasswordBox
            {
                // Открытие нового окна
                HomeScreen homeScreen = new HomeScreen();
                homeScreen.Show();
                this.Close(); // Закрыть текущее окно
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

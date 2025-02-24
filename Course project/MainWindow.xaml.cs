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
                        LoginButton.Content = "Войти"; // Изменение текста кнопки
                        break;
                    case "English":
                        TextBlockTitle.Text = "Authorization";
                        TextBlockLogin.Text = "Login:";
                        TextBlockPassword.Text = "Password:";
                        TextBlocklanguage.Text = "Select a language";
                        LoginButton.Content = "Login"; // Изменение текста кнопки
                        break;
                    case "Español":
                        TextBlockTitle.Text = "Autorización";
                        TextBlockLogin.Text = "Usuario:";
                        TextBlockPassword.Text = "Contraseña:";
                        TextBlocklanguage.Text = "Seleccione un idioma";
                        LoginButton.Content = "Iniciar sesión"; // Изменение текста кнопки
                        break;
                }

                // Сброс текста сообщения об ошибках при смене языка
                ErrorMessageTextBlock.Text = string.Empty;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка логина и пароля
            if (LoginTextBox.Text == "admin" && PasswordTextBox.Password == "admin")
            {
                App.Current.Properties["UserRole"] = "admin"; // Сохранение роли пользователя
                HomeScreen homeScreen = new HomeScreen();
                homeScreen.Show();
                this.Close(); // Закрыть текущее окно
            }
            else if (LoginTextBox.Text == "user" && PasswordTextBox.Password == "user")
            {
                App.Current.Properties["UserRole"] = "user"; // Сохранение роли пользователя
                HomeScreen2 homeScreen2 = new HomeScreen2();
                homeScreen2.Show();
                this.Close(); // Закрыть текущее окно
            }
            else
            {
                // Сообщение об ошибке
                ShowErrorMessage();
            }
        }

        private void ShowErrorMessage()
        {
            // Получение текущего языка из ComboBox
            string selectedLanguage = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string errorMessage = string.Empty;

            // Сообщение об ошибке в зависимости от выбранного языка
            switch (selectedLanguage)
            {
                case "Русский":
                    errorMessage = "Неверный логин или пароль!";
                    break;
                case "English":
                    errorMessage = "Incorrect login or password!";
                    break;
                case "Español":
                    errorMessage = "¡Usuario o contraseña incorrectos!";
                    break;
            }

            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

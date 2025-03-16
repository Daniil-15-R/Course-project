using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                LanguageManager.SetLanguage(selectedLanguage); // Устанавливаем текущий язык

                switch (selectedLanguage)
                {
                    case "Русский":
                        SetLanguageToRussian();
                        break;
                    case "English":
                        SetLanguageToEnglish();
                        break;
                    case "Español":
                        SetLanguageToSpanish();
                        break;
                }

                // Очистка сообщения об ошибке при изменении языка
                ErrorMessageTextBlock.Text = string.Empty;
            }
        }

        private void SetLanguageToRussian()
        {
            TextBlockTitle.Text = "Авторизация";
            TextBlockLogin.Text = "Логин:";
            TextBlockPassword.Text = "Пароль:";
            TextBlocklanguage.Text = "Выберите язык";
            LoginButton.Content = "Войти";
            RegistrationButton.Content = "Регистрация";// Change button text
        }

        private void SetLanguageToEnglish()
        {
            TextBlockTitle.Text = "Authorization";
            TextBlockLogin.Text = "Login:";
            TextBlockPassword.Text = "Password:";
            TextBlocklanguage.Text = "Select a language";
            LoginButton.Content = "Login";
            RegistrationButton.Content = "Registration";// Change button text
        }

        private void SetLanguageToSpanish()
        {
            TextBlockTitle.Text = "Autorización";
            TextBlockLogin.Text = "Usuario:";
            TextBlockPassword.Text = "Contraseña:";
            TextBlocklanguage.Text = "Seleccione un idioma";
            LoginButton.Content = "Iniciar sesión";
            RegistrationButton.Content = "Registro";// Change button text
        }

        private string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            if (Auth(login, password))
            {
                // Успешная авторизация
                try
                {
                    using (var db = new Entities())
                    {
                        var user = db.Users.AsNoTracking().FirstOrDefault(u =>
                            u.Login == login && (u.password == password || u.password == GetHash(password)));

                        if (user != null)
                        {
                            // Сохранение роли пользователя
                            App.Current.Properties["UserRole"] = user.role;

                            // Навигация в зависимости от роли
                            if (user.role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                var adminWindow = new HomeScreen(user);
                                adminWindow.Show();
                            }
                            else
                            {
                                var userWindow = new HomeScreen2(user);
                                userWindow.Show();
                            }

                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Произошла ошибка: {ex.Message}");
                }
            }
            else
            {
                ShowErrorMessage("Неверный логин или пароль");
            }
        }

        private void ShowErrorMessage(string message)
        {
            string selectedLanguage = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string errorMessage = string.Empty;

            switch (selectedLanguage)
            {
                case "Русский":
                    errorMessage = message;
                    break;
                case "English":
                    errorMessage = "Error: " + message;
                    break;
                case "Español":
                    errorMessage = "Error: " + message;
                    break;
            }

            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationScreen screen = new RegistrationScreen();
            screen.Show();
            this.Close();
        }
        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            string hashedPassword = GetHash(password);

            using (var db = new Entities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u =>
                    u.Login == login && (u.password == password || u.password == hashedPassword));

                return user != null;
            }
        }
    }
}
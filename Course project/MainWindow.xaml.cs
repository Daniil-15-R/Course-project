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
            // Проверка на наличие логина и пароля
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                ShowErrorMessage("Введите логин и пароль!");
                return;
            }

            string plainPassword = PasswordTextBox.Password; // Оригинальный пароль
            string hashedPassword = GetHash(plainPassword); // Хэшированный пароль

            try
            {
                using (var db = new Entities1())
                {
                    // Попытка найти пользователя с оригинальным паролем
                    var user = db.Users.AsNoTracking().FirstOrDefault(u =>
                        u.Login == LoginTextBox.Text && u.password == plainPassword);

                    if (user == null)
                    {
                        // Если не найден, попытаемся найти с хешированным паролем
                        user = db.Users.AsNoTracking().FirstOrDefault(u =>
                            u.Login == LoginTextBox.Text && u.password == hashedPassword);
                    }

                    if (user == null)
                    {
                        ShowErrorMessage("Пользователь с такими данными не найден!");
                        return;
                    }

                    // Сохранение роли пользователя в свойствах приложения
                    App.Current.Properties["UserRole"] = user.role;

                    // Навигация в зависимости от роли пользователя
                    if (user.role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var adminWindow = new HomeScreen(user);
                        adminWindow.Show();
                    }
                    else // Для всех остальных ролей (User, Employee, Volunteer)
                    {
                        var userWindow = new HomeScreen2(user);
                        userWindow.Show();
                    }

                    this.Close(); // Закрыть текущее окно
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Произошла ошибка: {ex.Message}");
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
    }
}
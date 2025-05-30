﻿using System;
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
        /// <summary>
        /// Обрабатывает изменение выбранного языка в комбобоке
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Аргументы события выбора</param>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedLanguage = selectedItem.Content.ToString();
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

                ErrorMessageTextBlock.Text = string.Empty;
            }
        }

        private void SetLanguageToRussian()
        {
            TextBlockTitle.Text = "Авторизация";
            TextBlockLogin.Text = "Логин:";
            TextBlockPassword.Text = "Пароль:";
            TextBlocklanguage.Text = "Выберите язык";
            LoginButton.Content = "Войти"; // Change button text
        }

        private void SetLanguageToEnglish()
        {
            TextBlockTitle.Text = "Authorization";
            TextBlockLogin.Text = "Login:";
            TextBlockPassword.Text = "Password:";
            TextBlocklanguage.Text = "Select a language";
            LoginButton.Content = "Login"; // Change button text
        }

        private void SetLanguageToSpanish()
        {
            TextBlockTitle.Text = "Autorización";
            TextBlockLogin.Text = "Usuario:";
            TextBlockPassword.Text = "Contraseña:";
            TextBlocklanguage.Text = "Seleccione un idioma";
            LoginButton.Content = "Iniciar sesión"; // Change button text
        }
        /// <summary>
        /// Генерирует SHA1 хэш для переданного пароля
        /// </summary>
        /// <param name="password">Пароль для хэширования</param>
        /// <returns>Хэшированная строка в шестнадцатеричном формате</returns>
        private string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
        /// <summary>
        /// Обрабатывает нажатие кнопки входа в систему
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Аргументы события нажатия кнопки</param>
        /// <exception cref="System.Exception">
        /// Может выбрасывать исключения при работе с базой данных
        /// </exception>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                ShowErrorMessage("Введите логин и пароль!");
                return;
            }

            string plainPassword = PasswordTextBox.Password;
            string hashedPassword = GetHash(plainPassword);

            try
            {
                using (var db = new Entities())
                {
                    var user = db.Users.AsNoTracking().FirstOrDefault(u =>
                        u.Login == LoginTextBox.Text && u.password == plainPassword);

                    if (user == null)
                    {
                        user = db.Users.AsNoTracking().FirstOrDefault(u =>
                            u.Login == LoginTextBox.Text && u.password == hashedPassword);
                    }

                    if (user == null)
                    {
                        ShowErrorMessage("Пользователь с такими данными не найден!");
                        return;
                    }

                    App.Current.Properties["UserRole"] = user.role;

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

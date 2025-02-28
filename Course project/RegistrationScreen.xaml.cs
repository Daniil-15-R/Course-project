using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Validation;
using System.Security.Cryptography;

namespace Course_project
{
    public partial class RegistrationScreen : Window
    {
        public RegistrationScreen()
        {
            InitializeComponent();
            UpdateLanguage(); // Обновляем язык при загрузке
        }

        public void UpdateLanguage()
        {
            string selectedLanguage = LanguageManager.CurrentLanguage;

            switch (selectedLanguage)
            {
                case "Русский":
                    TextBlockTitle.Text = "Регистрация"; // Заголовок
                    TextBlockLog.Text = "Логин:"; // Метка для логина
                    TextBlockPas.Text = "Пароль:"; // Метка для пароля
                    TextBlockEmail.Text = "Email:"; // Метка для email
                    TextBlockPhone.Text = "Телефон:"; // Метка для телефона
                    RegisterButton.Content = "Зарегистрироваться"; // Кнопка регистрации
                    BackButton.Content = "Назад"; // Кнопка назад
                                                  // Добавьте остальные поля и метки
                    break;

                case "English":
                    TextBlockTitle.Text = "Registration"; // Заголовок
                    TextBlockLog.Text = "Login:"; // Метка для логина
                    TextBlockPas.Text = "Password:"; // Метка для пароля
                    TextBlockEmail.Text = "Email:"; // Метка для email
                    TextBlockPhone.Text = "Phone:"; // Метка для телефона
                    RegisterButton.Content = "Register"; // Кнопка регистрации
                    BackButton.Content = "Back"; // Кнопка назад
                    break;

                case "Español":
                    TextBlockTitle.Text = "Registro"; // Заголовок
                    TextBlockLog.Text = "Usuario:"; // Метка для логина
                    TextBlockPas.Text = "Contraseña:"; // Метка для пароля
                    TextBlockEmail.Text = "Correo electrónico:"; // Метка для email
                    TextBlockPhone.Text = "Teléfono:"; // Метка для телефона
                    RegisterButton.Content = "Registrarse"; // Кнопка регистрации
                    BackButton.Content = "Atrás"; // Кнопка назад
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (LogText.Text.Length > 0)
            {
                using (var db = new Entities1())
                {
                    var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == LogText.Text);
                    if (user != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return;
                    }
                }
            }

            bool en = true;
            bool number = false;

            for (int i = 0; i < PasswordTextBox.Password.Length; i++)
            {
                if (PasswordTextBox.Password[i] >= 'A' && PasswordTextBox.Password[i] <= 'Z') en = false;
                if (PasswordTextBox.Password[i] >= '0' && PasswordTextBox.Password[i] <= '9') number = true;
            }

            var regex = new Regex(@"^\+7\d{10}$");

            StringBuilder errors = new StringBuilder();

            if (PasswordTextBox.Password.Length < 6) errors.AppendLine("Пароль должен быть больше 6 символов");
            if (!regex.IsMatch(PhoneText.Text)) errors.AppendLine("Укажите номер телефона в формате +7XXXXXXXXXX");
            if (!en) errors.AppendLine("Пароль должен быть на английском языке");
            if (!number) errors.AppendLine("Пароль должен содержать хотя бы одну цифру");
            if (!IsValidEmail(EmailText.Text)) errors.AppendLine("Введен некорректный email");

            try
            {
                using (Entities1 db = new Entities1())
                {
                    // Сохраняем пользователя в таблице Users
                    Users userObject = new Users
                    {
                        Login = LogText.Text,
                        password = GetHash(PasswordTextBox.Password),
                        role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString(), // Получаем выбранную роль
                    };
                    db.Users.Add(userObject);
                    db.SaveChanges();

                    // Получаем id только что добавленного пользователя
                    int userId = userObject.id;

                    ShelterEmployees shelterEmployee = new ShelterEmployees
                    {
                        FIO = NameText.Text + " " + Surnametext.Text,
                        email = EmailText.Text,
                        phone = PhoneText.Text,
                        SNILS = TextSNILS.Text,
                        date_of_birth = DataPicker.SelectedDate ?? DateTime.Now, // Значение по умолчанию
                        state_number_of_car = TextCar.Text,
                        place_of_registration = TextAddress.Text,
                        actual_address = TextAddress.Text,
                        passport = TextPassport.Text,
                        id = userId
                    };
                    db.ShelterEmployees.Add(shelterEmployee);

                    db.SaveChanges();
                    MessageBox.Show("Вы успешно зарегистрировались!", "Успешно!", MessageBoxButton.OK);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                MessageBox.Show(sb.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
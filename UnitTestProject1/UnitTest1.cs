/*using Course_project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class RegistrationTests
    {
        // Тестовые данные
        private const string ValidLogin = "testuser";
        private const string ValidPassword = "Test123!";
        private const string ValidEmail = "test@example.com";
        private const string ValidPhone = "+71234567890";

        [TestMethod]
        public void RegisterTest_Success_WithValidData()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin + Guid.NewGuid().ToString().Substring(0, 4),
                Password = ValidPassword,
                Email = ValidEmail,
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsTrue(result.IsSuccess, "Регистрация должна быть успешной с корректными данными");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithExistingLogin()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin,
                Password = ValidPassword,
                Email = ValidEmail,
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с существующим логином");
            Assert.IsTrue(result.ErrorMessage.Contains("логин"), "Должно быть сообщение о занятом логине");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithInvalidEmail()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin + Guid.NewGuid().ToString().Substring(0, 4),
                Password = ValidPassword,
                Email = "invalid-email",
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с неверным email");
            Assert.IsTrue(result.ErrorMessage.Contains("email"), "Должно быть сообщение о неверном email");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithShortPassword()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin + Guid.NewGuid().ToString().Substring(0, 4),
                Password = "123",
                Email = ValidEmail,
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с коротким паролем");
            Assert.IsTrue(result.ErrorMessage.Contains("пароль"), "Должно быть сообщение о коротком пароле");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithInvalidPhone()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin + Guid.NewGuid().ToString().Substring(0, 4),
                Password = ValidPassword,
                Email = ValidEmail,
                Phone = "1234567890",
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с неверным телефоном");
            Assert.IsTrue(result.ErrorMessage.Contains("телефон"), "Должно быть сообщение о неверном телефоне");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithCyrillicInLogin()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = "пользователь",
                Password = ValidPassword,
                Email = ValidEmail,
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с кириллицей в логине");
            Assert.IsTrue(result.ErrorMessage.Contains("английском"), "Должно быть сообщение о необходимости использовать латинские символы");
        }

        [TestMethod]
        public void RegisterTest_Fail_WithPasswordWithoutNumbers()
        {
            // Arrange
            var registrationService = new RegistrationService();
            var userData = new UserRegistrationData
            {
                Login = ValidLogin + Guid.NewGuid().ToString().Substring(0, 4),
                Password = "PasswordWithoutNumbers",
                Email = ValidEmail,
                Phone = ValidPhone,
                Name = "Test",
                Surname = "User"
            };

            // Act
            var result = registrationService.TryRegister(userData);

            // Assert
            Assert.IsFalse(result.IsSuccess, "Регистрация должна завершиться ошибкой с паролем без цифр");
            Assert.IsTrue(result.ErrorMessage.Contains("цифру"), "Должно быть сообщение о необходимости цифр в пароле");
        }
    }

    [TestClass]
    public class AuthorizationTests
    {
        [TestMethod]
        public void AuthTest_Success_WithValidCredentials()
        {
            // Arrange
            var authService = new AuthService();
            string validLogin = "admin";
            string validPassword = "admin123";

            // Act
            bool result = authService.Auth(validLogin, validPassword);

            // Assert
            Assert.IsTrue(result, "Авторизация должна быть успешной с корректными данными");
        }

        [TestMethod]
        public void AuthTest_Fail_WithInvalidPassword()
        {
            // Arrange
            var authService = new AuthService();
            string validLogin = "user123";
            string invalidPassword = "wrongpassword";

            // Act
            bool result = authService.Auth(validLogin, invalidPassword);

            // Assert
            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой с неверным паролем");
        }

        [TestMethod]
        public void AuthTest_Fail_WithEmptyFields()
        {
            // Arrange
            var authService = new AuthService();
            string emptyLogin = "";
            string emptyPassword = "";

            // Act
            bool result = authService.Auth(emptyLogin, emptyPassword);

            // Assert
            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой с пустыми полями");
        }

        [TestMethod]
        public void AuthTest_Fail_WithCapsLockPassword()
        {
            // Arrange
            var authService = new AuthService();
            string validLogin = "user123";
            string capsPassword = "PASSWORD123";

            // Act
            bool result = authService.Auth(validLogin, capsPassword);

            // Assert
            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой с паролем в CAPS LOCK");
        }

        [TestMethod]
        public void AuthTest_AccountLock_AfterMultipleFailedAttempts()
        {
            // Arrange
            var authService = new AuthService();
            string validLogin = "user123";
            string invalidPassword = "wrongpass";
            int maxAttempts = 3;

            // Act
            for (int i = 0; i < maxAttempts; i++)
            {
                authService.Auth(validLogin, invalidPassword);
            }

            // Assert
            Assert.IsTrue(authService.IsAccountLocked(validLogin), "Учетная запись должна быть заблокирована после 3 неудачных попыток");
        }
    }

    // Классы для тестирования
    public class RegistrationService
    {
        public RegistrationResult TryRegister(UserRegistrationData userData)
        {
            var result = new RegistrationResult();

            // Валидация данных
            if (string.IsNullOrEmpty(userData.Login))
            {
                result.ErrorMessage = "Логин не может быть пустым";
                return result;
            }

            if (userData.Password.Length < 6)
            {
                result.ErrorMessage = "Пароль должен быть не менее 6 символов";
                return result;
            }

            if (!IsValidEmail(userData.Email))
            {
                result.ErrorMessage = "Введен некорректный email";
                return result;
            }

            if (!userData.Phone.StartsWith("+7") || userData.Phone.Length != 12)
            {
                result.ErrorMessage = "Телефон должен быть в формате +7XXXXXXXXXX";
                return result;
            }

            // Проверка на кириллицу в логине
            if (ContainsCyrillic(userData.Login))
            {
                result.ErrorMessage = "Логин должен содержать только символы английского алфавита";
                return result;
            }

            // Проверка на отсутствие цифр в пароле
            if (!userData.Password.Any(char.IsDigit))
            {
                result.ErrorMessage = "Пароль должен содержать хотя бы одну цифру";
                return result;
            }

            // Проверка существующего пользователя (заглушка)
            if (userData.Login == "testuser")
            {
                result.ErrorMessage = "Пользователь с таким логином уже существует";
                return result;
            }

            result.IsSuccess = true;
            return result;
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

        private bool ContainsCyrillic(string text)
        {
            return text.Any(c => c >= 'А' && c <= 'я');
        }
    }

    public class AuthService
    {
        private Dictionary<string, int> failedAttempts = new Dictionary<string, int>();

        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Проверка блокировки учетной записи
            if (IsAccountLocked(login))
            {
                return false;
            }

            // Проверка CAPS LOCK (заглушка)
            if (password == password.ToUpper() && password.Any(char.IsLetter))
            {
                return false;
            }

            // Проверка учетных данных (заглушка)
            if (login == "admin" && password == "admin123")
            {
                return true;
            }

            if (login == "user123" && password == "password123")
            {
                return true;
            }

            // Увеличиваем счетчик неудачных попыток
            if (!failedAttempts.ContainsKey(login))
            {
                failedAttempts[login] = 0;
            }
            failedAttempts[login]++;

            return false;
        }

        public bool IsAccountLocked(string login)
        {
            return failedAttempts.ContainsKey(login) && failedAttempts[login] >= 3;
        }
    }

    public class UserRegistrationData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}*/
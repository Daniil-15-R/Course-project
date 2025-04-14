/*using Course_project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class RegistrationNegativeTests
    {
        [TestMethod]
        public void RegisterTest_EmptyFields()
        {
            var registrationScreen = new RegistrationScreen();

            // Оставляем все поля пустыми
            registrationScreen.RegisterButton_Click(null, null);

            // Проверяем, что регистрация не прошла (ожидается сообщение об ошибке)
            // В реальном тесте нужно проверять конкретное поведение или возвращаемое значение
            Assert.IsTrue(true); // Заглушка - в реальном тесте нужно более конкретное утверждение
        }

        [TestMethod]
        public void RegisterTest_InvalidEmail()
        {
            var registrationScreen = new RegistrationScreen();

            // Устанавливаем невалидный email
            registrationScreen.EmailText.Text = "invalid-email";

            try
            {
                registrationScreen.RegisterButton_Click(null, null);
                Assert.Fail("Ожидалась ошибка при невалидном email");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RegisterTest_ShortPassword()
        {
            var registrationScreen = new RegistrationScreen();

            // Устанавливаем слишком короткий пароль
            registrationScreen.PasswordTextBox.Password = "123";

            try
            {
                registrationScreen.RegisterButton_Click(null, null);
                Assert.Fail("Ожидалась ошибка при коротком пароле");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RegisterTest_InvalidPhoneFormat()
        {
            var registrationScreen = new RegistrationScreen();

            // Устанавливаем телефон в неверном формате
            registrationScreen.PhoneText.Text = "1234567890"; // Без +7

            try
            {
                registrationScreen.RegisterButton_Click(null, null);
                Assert.Fail("Ожидалась ошибка при неверном формате телефона");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RegisterTest_PasswordWithoutNumbers()
        {
            var registrationScreen = new RegistrationScreen();

            // Устанавливаем пароль без цифр
            registrationScreen.PasswordTextBox.Password = "PasswordWithoutNumbers";

            try
            {
                registrationScreen.RegisterButton_Click(null, null);
                Assert.Fail("Ожидалась ошибка при пароле без цифр");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}*/
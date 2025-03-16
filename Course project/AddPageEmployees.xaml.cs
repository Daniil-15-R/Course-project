using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // Для работы с диалоговым окном выбора файла
using System.IO; // Для работы с файлами

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для AddPageEmployees.xaml
    /// </summary>
    public partial class AddPageEmployees : Page
    {
        private ShelterEmployees _employees = new ShelterEmployees();

        public AddPageEmployees(ShelterEmployees selectedEmployees)
        {
            InitializeComponent();

            if (selectedEmployees != null)
                _employees = selectedEmployees;

            DataContext = _employees;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка на корректность данных
            if (string.IsNullOrWhiteSpace(_employees.FIO))
                errors.AppendLine("Введите ФИО сотрудника!");
            if (string.IsNullOrWhiteSpace(_employees.passport))
                errors.AppendLine("Введите паспортные данные!");
            if (string.IsNullOrWhiteSpace(_employees.phone))
                errors.AppendLine("Введите номер телефона!");
            if (string.IsNullOrWhiteSpace(_employees.email))
                errors.AppendLine("Введите email!");
            if (string.IsNullOrWhiteSpace(_employees.place_of_registration))
                errors.AppendLine("Введите место регистрации!");
            if (string.IsNullOrWhiteSpace(_employees.actual_address))
                errors.AppendLine("Введите фактический адрес проживания!");
            if (string.IsNullOrWhiteSpace(_employees.date_of_birth.ToString())) // Проверка даты рождения
                errors.AppendLine("Введите дату рождения!");
            if (string.IsNullOrWhiteSpace(_employees.SNILS))
                errors.AppendLine("Введите СНИЛС!");
            if (string.IsNullOrWhiteSpace(_employees.state_number_of_car))
                errors.AppendLine("Введите государственный номер автомобиля!");

            // Если есть ошибки, показываем их и прерываем выполнение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Если id сотрудника равен 0, добавляем новую запись
            if (_employees.id == 0)
                Entities.GetContext().ShelterEmployees.Add(_employees);

            try
            {
                // Сохраняем изменения в базе данных
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены");

                // Возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Выводим сообщение об ошибке
            }
        }

        // Обработчик для кнопки выбора фотографии
        private void ButtonSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному файлу
                string filePath = openFileDialog.FileName;

                // Загружаем изображение в объект ShelterEmployees
                _employees.image = File.ReadAllBytes(filePath);

                // Обновляем привязку изображения
                var imageBinding = EmployeeImage.GetBindingExpression(Image.SourceProperty);
                imageBinding?.UpdateTarget();
            }
        }
    }
}
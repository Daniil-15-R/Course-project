using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // Для работы с диалоговым окном выбора файла
using System.IO; // Для работы с файлами

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для AddPageDog.xaml
    /// </summary>
    public partial class AddPageDog : Page
    {
        private Dogs _dogs = new Dogs();

        public AddPageDog(Dogs selectedDogs)
        {
            InitializeComponent();

            if (selectedDogs != null)
                _dogs = selectedDogs;

            DataContext = _dogs;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка на корректность данных
            if (string.IsNullOrWhiteSpace(_dogs.nickname))
                errors.AppendLine("Введите кличку собаки!");
            if (string.IsNullOrWhiteSpace(_dogs.gender))
                errors.AppendLine("Введите пол!");
            if (_dogs.age <= 0) // Проверка, что возраст больше 0
                errors.AppendLine("Введите корректный возраст!");
            if (string.IsNullOrWhiteSpace(_dogs.additional_information))
                errors.AppendLine("Введите дополнительную информацию!");

            // Если есть ошибки, показываем их и прерываем выполнение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Если id собаки равен 0, добавляем новую запись
            if (_dogs.id == 0)
                Entities.GetContext().Dogs.Add(_dogs);

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

                // Загружаем изображение в объект Dogs
                _dogs.image = File.ReadAllBytes(filePath);

                // Обновляем привязку изображения
                var imageBinding = image.GetBindingExpression(Image.SourceProperty);
                imageBinding?.UpdateTarget();
            }
        }
    }
}
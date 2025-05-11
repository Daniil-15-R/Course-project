using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class AddMedicinePage : Page
    {
        private Medicines _medicines = new Medicines();
        public AddMedicinePage(Medicines selectedMedicines)
        {
            InitializeComponent();

            if (selectedMedicines != null)
                _medicines = selectedMedicines;

            DataContext = _medicines;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка на корректность данных
            if (string.IsNullOrWhiteSpace(_medicines.name_of_medicine))
                errors.AppendLine("Введите название лекарства!");
            if (_medicines.dog_id <= 0) // Проверка, что выбран корректный Id собаки
                errors.AppendLine("Выберите собаку!");
            if (_medicines.cost <= 0) // Проверка, что цена больше 0
                errors.AppendLine("Введите корректную цену!");
            if (_medicines.purchase_date == default(DateTime))
                errors.AppendLine("Укажите дату!");

            if (_medicines.quantity < 0)
                errors.AppendLine("Количество не может быть отрицательной!");
            // Если есть ошибки, показываем их и прерываем выполнение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Если id лекарства равен 0, добавляем новую запись
            if (_medicines.id == 0)
                Entities.GetContext().Medicines.Add(_medicines);

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
    }
}
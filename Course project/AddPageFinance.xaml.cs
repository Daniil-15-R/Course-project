using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для AddPageFinance.xaml
    /// </summary>
    public partial class AddPageFinance : Page
    {
        private Accounting _accounting = new Accounting();
        public AddPageFinance(Accounting selectedFinance)
        {
            InitializeComponent();
            if (selectedFinance != null)
                _accounting = selectedFinance;
            DataContext = _accounting;
        }
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка даты
            if (_accounting.accounting_date == default(DateTime))
                errors.AppendLine("Укажите дату!");
            else if (_accounting.accounting_date < DateTime.Today)
                errors.AppendLine("Дата не может быть в прошлом!");

            // Проверка числовых полей (должны быть положительными числами)
            if (_accounting.salary < 0)
                errors.AppendLine("Зарплата не может быть отрицательной!");

            if (_accounting.utilities < 0)
                errors.AppendLine("Коммунальные услуги не могут быть отрицательными!");

            if (_accounting.taxes < 0)
                errors.AppendLine("Налоги не могут быть отрицательными!");

            if (_accounting.medicine_expenses < 0)
                errors.AppendLine("Затраты на лекарство не могут быть отрицательными!");

            if (_accounting.food_expenses < 0)
                errors.AppendLine("Затраты на еду не могут быть отрицательными!");

            if (_accounting.other_expenses < 0)
                errors.AppendLine("Затраты на прочие нужды не могут быть отрицательными!");

            if (_accounting.voluntary_contributions < 0)
                errors.AppendLine("Добровольный взнос не может быть отрицательным!");

            if (_accounting.total < 0)
                errors.AppendLine("Итог не может быть отрицательным!");

            // Проверка ID (если он должен быть положительным)
            if (_accounting.id <= 0)
                errors.AppendLine("Введите корректный ID!");

            // Если есть ошибки, показываем их и прерываем выполнение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Если это новая запись, добавляем её в контекст
                if (_accounting.id == 0)
                    Entities.GetContext().Accounting.Add(_accounting);

                // Сохраняем изменения в базе данных
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

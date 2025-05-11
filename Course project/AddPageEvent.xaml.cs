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
    /// Логика взаимодействия для AddPageEvent.xaml
    /// </summary>
    public partial class AddPageEvent : Page
    {
        private PlannedEvents _events = new PlannedEvents();
        public AddPageEvent(PlannedEvents selectedEvents)
        {
            InitializeComponent();
            if (selectedEvents != null)
                _events = selectedEvents;
            DataContext = _events;
        }
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_events.FIO))
                errors.AppendLine("Введите ФИО!");

            if (string.IsNullOrWhiteSpace(_events.title))
                errors.AppendLine("Введите название мероприятия!");

            if (_events.dog_id <= 0)
                errors.AppendLine("Введите корректный id собаки!");


            // Если есть ошибки, показываем их и прерываем выполнение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

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

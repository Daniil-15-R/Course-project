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
    /// Логика взаимодействия для AddPageNeeds.xaml
    /// </summary>
    public partial class AddPageNeeds : Page
    {
        private ShelterNeeds _need = new ShelterNeeds();
        public AddPageNeeds(ShelterNeeds selectedNeed)
        {
            InitializeComponent();
            if (selectedNeed != null)
                _need = selectedNeed;

            DataContext = _need;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка на корректность данных
            if (string.IsNullOrWhiteSpace(_need.title))
                errors.AppendLine("Введите название еды!");
            if (_need.cost <= 0) // Проверка, что цена больше 0
                errors.AppendLine("Введите корректную цену!");

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

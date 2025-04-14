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
    /// Логика взаимодействия для AddPageParasite.xaml
    /// </summary>
    public partial class AddPageParasite : Page
    {
        private ParasiteTreatmentSchedule _parasite = new ParasiteTreatmentSchedule();
        public AddPageParasite(ParasiteTreatmentSchedule selectedParasite)
        {
            InitializeComponent();
            if (selectedParasite != null)
                _parasite = selectedParasite;
            DataContext = _parasite;
        }
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_parasite.title))
                errors.AppendLine("введите название обработки!");

            if (_parasite.dog_id <= 0)
                errors.AppendLine("Введите корректный id собаки!");

            if (_parasite.id <= 0)
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

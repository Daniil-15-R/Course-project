using System;
using System.Text;
using System.Windows;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для AddPageFinance.xaml
    /// </summary>
    public partial class AddPageFinance : Window
    {
        private Accounting _accounting = new Accounting();

        public AddPageFinance(Accounting selectAccount)
        {
            InitializeComponent();
            if (selectAccount != null)
                _accounting = selectAccount;
            DataContext = _accounting;
        }
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Convert.ToString(_accounting.id))) errors.AppendLine("Введите Id!");
            //if ();
            if (_accounting.salary <= 0) errors.AppendLine("Введите корректную стоимость");
            if (_accounting.utilities <= 0) errors.AppendLine("Введите корректное число (коммульные услуги)!");
            if (_accounting.taxes <= 0) errors.AppendLine("Введите корректное число (налоги)!");
            if (_accounting.medicine_expenses <= 0) errors.AppendLine();
            if (_accounting.food_expenses <= 0) errors.AppendLine();
            if (_accounting.other_expenses <= 0) errors.AppendLine();
            if (_accounting.voluntary_contributions <= 0) errors.AppendLine();
            if (_accounting.total <= 0) errors.AppendLine();
        }
    }
}

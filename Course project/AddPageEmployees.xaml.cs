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

        }
    }
}

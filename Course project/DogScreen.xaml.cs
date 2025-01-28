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
using System.Windows.Shapes;

namespace Course_project
{
    /// <summary>
    /// Логика взаимодействия для DogScreen.xaml
    /// </summary>
    public partial class DogScreen : Window
    {
        private Window _previousWindow;

        public DogScreen(Window previousWindow)
        {
            InitializeComponent();
            _previousWindow = previousWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _previousWindow.Show(); // Показываем предыдущее окно
            this.Close();           // Закрываем текущее окно
        }
    }
}

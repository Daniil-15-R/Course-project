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
    /// Логика взаимодействия для NeedScreen.xaml
    /// </summary>
    public partial class NeedScreen : Window
    {
        public NeedScreen()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Здесь тоже нужно будет сохранить или получить текущего пользователя, если нужно
            this.Close();
        }
    }
}

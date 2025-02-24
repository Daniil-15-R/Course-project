using System.Windows;

namespace Course_project
{
    public partial class FinanceScreen : Window
    {
        public FinanceScreen()
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
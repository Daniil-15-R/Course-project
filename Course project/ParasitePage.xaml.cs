using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Course_project
{
    public partial class ParasitePage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public ParasitePage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateParasiteTreatments();
        }

        private void UpdateParasiteTreatments()
        {
            var currentTreatments = Entities.GetContext().ParasiteTreatmentSchedule.ToList();

            if (!string.IsNullOrWhiteSpace(SearchParasite.Text))
            {
                currentTreatments = currentTreatments.Where(x =>
                    x.title.ToLower().Contains(SearchParasite.Text.ToLower())).ToList();
            }

            if (SortParasite.SelectedIndex == 0)
            {
                currentTreatments = currentTreatments.OrderBy(x => x.title).ToList();
            }
            else if (SortParasite.SelectedIndex == 1)
            {
                currentTreatments = currentTreatments.OrderByDescending(x => x.title).ToList();
            }
            else if (SortParasite.SelectedIndex == 2)
            {
                currentTreatments = currentTreatments.OrderByDescending(x => x.date).ToList();
            }
            else if (SortParasite.SelectedIndex == 3)
            {
                currentTreatments = currentTreatments.OrderBy(x => x.date).ToList();
            }

            DataGridParasite.ItemsSource = currentTreatments;
        }

        private void SearchParasite_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateParasiteTreatments();
        }

        private void SortParasite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateParasiteTreatments();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchParasite.Text = "";
            SortParasite.SelectedIndex = -1;
            UpdateParasiteTreatments();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageParasite((sender as Button).DataContext as ParasiteTreatmentSchedule));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageParasite(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var treatmentsForRemoving = DataGridParasite.SelectedItems.Cast<ParasiteTreatmentSchedule>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {treatmentsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ParasiteTreatmentSchedule.RemoveRange(treatmentsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateParasiteTreatments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    new HomeScreen(_currentUser).Show();
                }
                else
                {
                    new HomeScreen2(_currentUser).Show();
                }
                Window.GetWindow(this)?.Close();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new MedicinePage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new WalkingPage(_currentUser, _userRole, true));
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new VacinationPage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new VacinationPage(_currentUser, _userRole, true));
            }
        }

        private void ParasitePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateParasiteTreatments();
            }
        }
    }
}
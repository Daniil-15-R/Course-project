using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Collections.Generic;

namespace Course_project
{
    public partial class EventPage : Page
    {
        private Users _currentUser;
        private string _userRole;
        private bool _fromHomeScreen2;

        public EventPage(Users currentUser, string userRole, bool fromHomeScreen2 = false)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;
            _fromHomeScreen2 = fromHomeScreen2;

            UpdateEvents();
        }

        private void UpdateEvents()
        {
            var currentEvents = Entities.GetContext().PlannedEvents.ToList();

            if (!string.IsNullOrWhiteSpace(SearchEvent.Text))
            {
                currentEvents = currentEvents.Where(x =>
                    x.title.ToLower().Contains(SearchEvent.Text.ToLower())).ToList();
            }

            if (SortEvent.SelectedIndex == 0)
            {
                currentEvents = currentEvents.OrderBy(x => x.title).ToList();
            }
            else if (SortEvent.SelectedIndex == 1)
            {
                currentEvents = currentEvents.OrderByDescending(x => x.title).ToList();
            }
            else if (SortEvent.SelectedIndex == 2)
            {
                currentEvents = currentEvents.OrderByDescending(x => x.date).ToList();
            }
            else if (SortEvent.SelectedIndex == 3)
            {
                currentEvents = currentEvents.OrderBy(x => x.date).ToList();
            }

            DataGridEvent.ItemsSource = currentEvents;
        }

        private void SearchEvent_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEvents();
        }

        private void SortEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEvents();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchEvent.Text = "";
            SortEvent.SelectedIndex = -1;
            UpdateEvents();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEvent((sender as Button).DataContext as PlannedEvents));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageEvent(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var eventsForRemoving = DataGridEvent.SelectedItems.Cast<PlannedEvents>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {eventsForRemoving.Count()} элементов?",
                "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().PlannedEvents.RemoveRange(eventsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    UpdateEvents();
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
                NavigationService.Navigate(new EmployeesPage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new DogPage(_currentUser, _userRole, true));
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.Navigate(new ParasitePage(_currentUser, _userRole));
            }
            else
            {
                NavigationService.Navigate(new WalkingPage(_currentUser, _userRole, true));
            }
        }

        private void EventPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateEvents();
            }
        }

        private void WordReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var wordApp = new Word.Application();
                Word.Document document = wordApp.Documents.Add();

                // Заголовок документа
                Word.Paragraph titleParagraph = document.Paragraphs.Add();
                Word.Range titleRange = titleParagraph.Range;
                titleRange.Text = "Отчет по мероприятиям";
                titleParagraph.set_Style("Заголовок 1");
                titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                titleRange.InsertParagraphAfter();

                // Дата генерации отчета
                Word.Paragraph dateParagraph = document.Paragraphs.Add();
                Word.Range dateRange = dateParagraph.Range;
                dateRange.Text = $"Дата формирования отчета: {DateTime.Now:dd.MM.yyyy HH:mm}";
                dateParagraph.set_Style("Заголовок 3");
                dateRange.InsertParagraphAfter();
                document.Paragraphs.Add();

                // Таблица с данными
                var events = DataGridEvent.ItemsSource as IEnumerable<PlannedEvents>;
                if (events != null && events.Any())
                {
                    Word.Paragraph tableParagraph = document.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    Word.Table eventsTable = document.Tables.Add(tableRange, events.Count() + 1, 4);

                    eventsTable.Borders.InsideLineStyle = eventsTable.Borders.OutsideLineStyle =
                        Word.WdLineStyle.wdLineStyleSingle;
                    eventsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    // Заголовки таблицы
                    eventsTable.Cell(1, 1).Range.Text = "ФИО";
                    eventsTable.Cell(1, 2).Range.Text = "Название мероприятия";
                    eventsTable.Cell(1, 3).Range.Text = "ID собаки";
                    eventsTable.Cell(1, 4).Range.Text = "Дата проведения";

                    // Стиль заголовков
                    eventsTable.Rows[1].Range.Font.Bold = 1;
                    eventsTable.Rows[1].Range.Font.Name = "Times New Roman";
                    eventsTable.Rows[1].Range.Font.Size = 12;
                    eventsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    // Заполнение данными
                    int row = 2;
                    foreach (var ev in events)
                    {
                        eventsTable.Cell(row, 1).Range.Text = ev.FIO ?? "-";
                        eventsTable.Cell(row, 2).Range.Text = ev.title ?? "-";
                        eventsTable.Cell(row, 3).Range.Text = ev.dog_id.ToString();
                        eventsTable.Cell(row, 4).Range.Text = ev.date.ToString("dd.MM.yyyy");

                        // Стиль данных
                        for (int col = 1; col <= 4; col++)
                        {
                            eventsTable.Cell(row, col).Range.Font.Name = "Times New Roman";
                            eventsTable.Cell(row, col).Range.Font.Size = 12;
                        }
                        row++;
                    }

                    // Автоподбор ширины столбцов
                    eventsTable.Columns.AutoFit();

                    // Статистика
                    document.Paragraphs.Add();
                    Word.Paragraph statsParagraph = document.Paragraphs.Add();
                    Word.Range statsRange = statsParagraph.Range;
                    statsRange.Text = $"Всего мероприятий: {events.Count()}\n" +
                                    $"Самое раннее мероприятие: {events.Min(x => x.date):dd.MM.yyyy}\n" +
                                    $"Самое позднее мероприятие: {events.Max(x => x.date):dd.MM.yyyy}";
                    statsParagraph.set_Style("Заголовок 3");
                }
                else
                {
                    Word.Paragraph noDataParagraph = document.Paragraphs.Add();
                    Word.Range noDataRange = noDataParagraph.Range;
                    noDataRange.Text = "Нет данных о мероприятиях для отображения";
                    noDataParagraph.set_Style("Обычный");
                }

                wordApp.Visible = true;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Отчет_по_мероприятиям_{DateTime.Now:yyyyMMdd_HHmmss}";
                document.SaveAs2(Path.Combine(desktopPath, $"{fileName}.docx"));
                document.SaveAs2(Path.Combine(desktopPath, $"{fileName}.pdf"), Word.WdExportFormat.wdExportFormatPDF);

                MessageBox.Show($"Отчет успешно сгенерирован и сохранен на рабочем столе!\nФайлы: {fileName}.docx и {fileName}.pdf",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExcelReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.Sheets[1] as Excel.Worksheet;

                // Заголовки
                worksheet.Cells[1, 1] = "ФИО";
                worksheet.Cells[1, 2] = "Название мероприятия";
                worksheet.Cells[1, 3] = "ID собаки";
                worksheet.Cells[1, 4] = "Дата проведения";

                // Данные
                var events = DataGridEvent.ItemsSource as IEnumerable<PlannedEvents>;
                if (events != null && events.Any())
                {
                    int row = 2;
                    foreach (var ev in events)
                    {
                        worksheet.Cells[row, 1] = ev.FIO ?? "-";
                        worksheet.Cells[row, 2] = ev.title ?? "-";
                        worksheet.Cells[row, 3] = ev.dog_id;
                        worksheet.Cells[row, 4] = ev.date.ToString("dd.MM.yyyy");
                        row++;
                    }

                    // Статистика
                    worksheet.Cells[row + 1, 1] = "Всего мероприятий:";
                    worksheet.Cells[row + 1, 2] = events.Count();
                    worksheet.Cells[row + 2, 1] = "Самое раннее мероприятие:";
                    worksheet.Cells[row + 2, 2] = events.Min(x => x.date).ToString("dd.MM.yyyy");
                    worksheet.Cells[row + 3, 1] = "Самое позднее мероприятие:";
                    worksheet.Cells[row + 3, 2] = events.Max(x => x.date).ToString("dd.MM.yyyy");
                }

                // Форматирование
                Excel.Range headerRange = worksheet.Range["A1:D1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                // Автоширина столбцов
                worksheet.Columns.AutoFit();

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, $"Отчет_по_мероприятиям_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                workbook.SaveAs(fileName);

                MessageBox.Show($"Excel-отчет успешно сгенерирован и сохранен на рабочем столе!\nФайл: {fileName}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации Excel-отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
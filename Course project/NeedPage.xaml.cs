using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace Course_project
{
    public partial class NeedPage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public NeedPage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            UpdateNeeds();
        }

        private decimal GetDecimalValue(decimal? value)
        {
            return value.HasValue ? value.Value : 0m;
        }

        private void UpdateNeeds()
        {
            var currentNeeds = Entities.GetContext().ShelterNeeds.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(SearchNeed.Text))
            {
                currentNeeds = currentNeeds.Where(x =>
                    x.title.ToLower().Contains(SearchNeed.Text.ToLower())).ToList();
            }

            // Сортировка
            switch (SortNeed.SelectedIndex)
            {
                case 0:
                    currentNeeds = currentNeeds.OrderBy(x => x.title).ToList();
                    break;
                case 1:
                    currentNeeds = currentNeeds.OrderByDescending(x => x.title).ToList();
                    break;
                case 2:
                    currentNeeds = currentNeeds.OrderBy(x => GetDecimalValue(x.cost)).ToList();
                    break;
                case 3:
                    currentNeeds = currentNeeds.OrderByDescending(x => GetDecimalValue(x.cost)).ToList();
                    break;
            }

            DataGridNeeds.ItemsSource = currentNeeds;
        }

        private void WordReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var allNeeds = Entities.GetContext().ShelterNeeds.ToList();

                var wordApp = new Word.Application();
                Word.Document document = wordApp.Documents.Add();

                // Заголовок документа
                Word.Paragraph titleParagraph = document.Paragraphs.Add();
                Word.Range titleRange = titleParagraph.Range;
                titleRange.Text = "Отчет о потребностях приюта";
                titleParagraph.set_Style("Заголовок 1");
                titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                titleRange.InsertParagraphAfter();

                // Дата генерации отчета
                Word.Paragraph dateParagraph = document.Paragraphs.Add();
                Word.Range dateRange = dateParagraph.Range;
                dateRange.Text = $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}";
                dateParagraph.set_Style("Заголовок 3");
                dateRange.InsertParagraphAfter();

                document.Paragraphs.Add();

                if (allNeeds.Any())
                {
                    // Создаем таблицу
                    Word.Paragraph tableParagraph = document.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    Word.Table needsTable = document.Tables.Add(tableRange, allNeeds.Count + 1, 3);

                    needsTable.Borders.InsideLineStyle = needsTable.Borders.OutsideLineStyle =
                        Word.WdLineStyle.wdLineStyleSingle;
                    needsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    // Заголовки таблицы
                    Word.Range cellRange = needsTable.Cell(1, 1).Range;
                    cellRange.Text = "Наименование";
                    cellRange = needsTable.Cell(1, 2).Range;
                    cellRange.Text = "Описание";
                    cellRange = needsTable.Cell(1, 3).Range;
                    cellRange.Text = "Стоимость";

                    // Форматирование заголовков
                    needsTable.Rows[1].Range.Font.Name = "Times New Roman";
                    needsTable.Rows[1].Range.Font.Size = 14;
                    needsTable.Rows[1].Range.Bold = 1;
                    needsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    // Заполнение таблицы данными
                    for (int i = 0; i < allNeeds.Count; i++)
                    {
                        var need = allNeeds[i];

                        cellRange = needsTable.Cell(i + 2, 1).Range;
                        cellRange.Text = need.title;
                        cellRange.Font.Name = "Times New Roman";
                        cellRange.Font.Size = 12;

                        cellRange = needsTable.Cell(i + 2, 3).Range;
                        cellRange.Text = $"{GetDecimalValue(need.cost):N2} руб.";
                        cellRange.Font.Name = "Times New Roman";
                        cellRange.Font.Size = 12;
                        cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    }

                    document.Paragraphs.Add();

                    // Итоговая сумма
                    decimal totalCost = allNeeds.Sum(n => GetDecimalValue(n.cost));
                    Word.Paragraph totalParagraph = document.Paragraphs.Add();
                    Word.Range totalRange = totalParagraph.Range;
                    totalRange.Text = $"Общая сумма потребностей: {totalCost:N2} руб.";
                    totalParagraph.set_Style("Заголовок 2");
                    totalRange.Font.Bold = 1;
                    totalRange.InsertParagraphAfter();
                }
                else
                {
                    Word.Paragraph noDataParagraph = document.Paragraphs.Add();
                    Word.Range noDataRange = noDataParagraph.Range;
                    noDataRange.Text = "Нет данных о потребностях";
                    noDataParagraph.set_Style("Заголовок 3");
                    noDataRange.InsertParagraphAfter();
                }

                wordApp.Visible = true;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Отчет_потребности_{DateTime.Now:yyyyMMdd_HHmmss}";
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
                worksheet.Cells[1, 1] = "Наименование";
                worksheet.Cells[1, 2] = "Описание";
                worksheet.Cells[1, 3] = "Стоимость";
                worksheet.Cells[1, 4] = "Приоритет";

                // Данные
                var allData = Entities.GetContext().ShelterNeeds.ToList();

                int row = 2;
                foreach (var item in allData)
                {
                    worksheet.Cells[row, 1] = item.title;
                    worksheet.Cells[row, 3] = GetDecimalValue(item.cost);
                    row++;
                }

                // Итоговая строка
                worksheet.Cells[row, 1] = "ИТОГО:";
                worksheet.Cells[row, 3] = allData.Sum(n => GetDecimalValue(n.cost));

                // Форматирование
                Excel.Range headerRange = worksheet.Range["A1:D1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                Excel.Range totalRange = worksheet.Range[$"A{row}:D{row}"];
                totalRange.Font.Bold = true;
                totalRange.Interior.Color = Excel.XlRgbColor.rgbLightBlue;

                // Автоширина столбцов
                worksheet.Columns.AutoFit();

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, $"Отчет_потребности_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
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

        private void SearchNeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateNeeds();
        }

        private void SortNeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateNeeds();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchNeed.Text = "";
            SortNeed.SelectedIndex = -1;
            UpdateNeeds();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageNeeds((sender as Button).DataContext as ShelterNeeds));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageNeeds(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var needsForRemoving = DataGridNeeds.SelectedItems.Cast<ShelterNeeds>().ToList();
            if (needsForRemoving.Count == 0)
            {
                MessageBox.Show("Выберите записи для удаления", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить {needsForRemoving.Count} записей?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().ShelterNeeds.RemoveRange(needsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateNeeds();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                var homeScreen = new HomeScreen(_currentUser);
                homeScreen.Show();
                Window.GetWindow(this)?.Close();
            }
        }

        private void NextButton_Cick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FinancePage(_currentUser, _userRole));
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FoodPage(_currentUser, _userRole));
        }

        private void NeedPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateNeeds();
            }
        }
    }
}
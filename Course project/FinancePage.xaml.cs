using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;

namespace Course_project
{
    public partial class FinancePage : Page
    {
        private Users _currentUser;
        private string _userRole;

        public FinancePage(Users currentUser, string userRole)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRole = userRole;

            InitializeChart();
            UpdateFinances();
        }

        private decimal GetDecimalValue(decimal? value)
        {
            return value.HasValue ? value.Value : 0m;
        }

        private void InitializeChart()
        {
            ChartPayments.Series.Clear();
            ChartPayments.ChartAreas.Clear();
            ChartPayments.Legends.Clear();

            var chartArea = new ChartArea("MainChartArea")
            {
                AxisX = { Title = "Категории расходов" },
                AxisY = { Title = "Сумма" }
            };
            ChartPayments.ChartAreas.Add(chartArea);

            var legend = new Legend
            {
                Name = "MainLegend",
                Docking = Docking.Top,
                IsDockedInsideChartArea = false
            };
            ChartPayments.Legends.Add(legend);

            var series = new Series("Финансы")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "N2",
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };
            ChartPayments.Series.Add(series);

            CmbUser.ItemsSource = Entities.GetContext().Users.ToList();
            CmbDiagram.ItemsSource = Enum.GetValues(typeof(SeriesChartType)).Cast<SeriesChartType>();
            CmbDiagram.SelectedItem = SeriesChartType.Column;
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (CmbUser.SelectedItem is Users selectedUser)
            {
                var series = ChartPayments.Series["Финансы"];
                if (CmbDiagram.SelectedItem is SeriesChartType chartType)
                {
                    series.ChartType = chartType;
                }
                series.Points.Clear();

                var finances = Entities.GetContext().Accounting
                    .Where(a => a.id == selectedUser.id)
                    .ToList();

                if (!finances.Any()) return;

                AddDataPoint(series, "Зарплата", finances.Sum(f => GetDecimalValue(f.salary)));
                AddDataPoint(series, "Коммунальные", finances.Sum(f => GetDecimalValue(f.utilities)));
                AddDataPoint(series, "Налоги", finances.Sum(f => GetDecimalValue(f.taxes)));
                AddDataPoint(series, "Лекарства", finances.Sum(f => GetDecimalValue(f.medicine_expenses)));
                AddDataPoint(series, "Еда", finances.Sum(f => GetDecimalValue(f.food_expenses)));
                AddDataPoint(series, "Прочие", finances.Sum(f => GetDecimalValue(f.other_expenses)));
                AddDataPoint(series, "Взносы", finances.Sum(f => GetDecimalValue(f.voluntary_contributions)));
            }
        }

        private void AddDataPoint(Series series, string category, decimal value)
        {
            if (value > 0)
            {
                var point = new DataPoint
                {
                    AxisLabel = category,
                    YValues = new[] { (double)value }
                };
                series.Points.Add(point);
            }
        }

        private void UpdateFinances()
        {
            var currentFinances = Entities.GetContext().Accounting.ToList();

            if (!string.IsNullOrWhiteSpace(SearchFinance.Text))
            {
                currentFinances = currentFinances.Where(x =>
                    x.accounting_date.ToString().Contains(SearchFinance.Text)).ToList();
            }

            switch (SortFinance.SelectedIndex)
            {
                case 0:
                    currentFinances = currentFinances.OrderByDescending(x => x.accounting_date).ToList();
                    break;
                case 1:
                    currentFinances = currentFinances.OrderBy(x => x.accounting_date).ToList();
                    break;
                case 2:
                    currentFinances = currentFinances.OrderBy(x => GetDecimalValue(x.total)).ToList();
                    break;
                case 3:
                    currentFinances = currentFinances.OrderByDescending(x => GetDecimalValue(x.total)).ToList();
                    break;
            }

            DataGridFinance.ItemsSource = currentFinances;
        }

        private void WordReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var allUsers = Entities.GetContext().Users.ToList();
                var allCategories = new[] { "Зарплата", "Коммунальные", "Налоги", "Лекарства", "Еда", "Прочие", "Взносы" };

                var wordApp = new Word.Application();
                Word.Document document = wordApp.Documents.Add();

                foreach (var user in allUsers)
                {
                    string userName = $"Пользователь ID: {user.id}";

                    Word.Paragraph userParagraph = document.Paragraphs.Add();
                    Word.Range userRange = userParagraph.Range;
                    userRange.Text = userName;
                    userParagraph.set_Style("Заголовок 1");
                    userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    userRange.InsertParagraphAfter();
                    document.Paragraphs.Add();

                    var userFinances = Entities.GetContext().Accounting
                        .Where(a => a.id == user.id)
                        .ToList();

                    if (userFinances.Any())
                    {
                        Word.Paragraph tableParagraph = document.Paragraphs.Add();
                        Word.Range tableRange = tableParagraph.Range;
                        Word.Table paymentsTable = document.Tables.Add(tableRange, allCategories.Length + 1, 2);

                        paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle =
                            Word.WdLineStyle.wdLineStyleSingle;
                        paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                        Word.Range cellRange = paymentsTable.Cell(1, 1).Range;
                        cellRange.Text = "Категория";
                        cellRange = paymentsTable.Cell(1, 2).Range;
                        cellRange.Text = "Сумма расходов";

                        paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                        paymentsTable.Rows[1].Range.Font.Size = 14;
                        paymentsTable.Rows[1].Range.Bold = 1;
                        paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        for (int i = 0; i < allCategories.Length; i++)
                        {
                            string category = allCategories[i];
                            decimal sum = GetCategorySum(userFinances, category);

                            cellRange = paymentsTable.Cell(i + 2, 1).Range;
                            cellRange.Text = category;
                            cellRange.Font.Name = "Times New Roman";
                            cellRange.Font.Size = 12;

                            cellRange = paymentsTable.Cell(i + 2, 2).Range;
                            cellRange.Text = $"{sum:N2} руб.";
                            cellRange.Font.Name = "Times New Roman";
                            cellRange.Font.Size = 12;
                        }

                        document.Paragraphs.Add();

                        var maxPayment = userFinances
                            .OrderByDescending(f => GetDecimalValue(f.utilities) + GetDecimalValue(f.taxes) +
                                      GetDecimalValue(f.medicine_expenses) + GetDecimalValue(f.food_expenses) +
                                      GetDecimalValue(f.other_expenses) + GetDecimalValue(f.voluntary_contributions))
                            .FirstOrDefault();

                        if (maxPayment != null)
                        {
                            decimal maxValue = GetDecimalValue(maxPayment.utilities) + GetDecimalValue(maxPayment.taxes) +
                                             GetDecimalValue(maxPayment.medicine_expenses) + GetDecimalValue(maxPayment.food_expenses) +
                                             GetDecimalValue(maxPayment.other_expenses) + GetDecimalValue(maxPayment.voluntary_contributions);

                            Word.Paragraph maxParagraph = document.Paragraphs.Add();
                            Word.Range maxRange = maxParagraph.Range;
                            maxRange.Text = $"Максимальные расходы: {maxValue:N2} руб. за {maxPayment.accounting_date:dd.MM.yyyy}";
                            maxParagraph.set_Style("Заголовок 3");
                            maxRange.Font.Color = Word.WdColor.wdColorDarkRed;
                            maxRange.InsertParagraphAfter();
                        }

                        var minPayment = userFinances
                            .OrderBy(f => GetDecimalValue(f.utilities) + GetDecimalValue(f.taxes) +
                                    GetDecimalValue(f.medicine_expenses) + GetDecimalValue(f.food_expenses) +
                                    GetDecimalValue(f.other_expenses) + GetDecimalValue(f.voluntary_contributions))
                            .FirstOrDefault();

                        if (minPayment != null)
                        {
                            decimal minValue = GetDecimalValue(minPayment.utilities) + GetDecimalValue(minPayment.taxes) +
                                             GetDecimalValue(minPayment.medicine_expenses) + GetDecimalValue(minPayment.food_expenses) +
                                             GetDecimalValue(minPayment.other_expenses) + GetDecimalValue(minPayment.voluntary_contributions);

                            Word.Paragraph minParagraph = document.Paragraphs.Add();
                            Word.Range minRange = minParagraph.Range;
                            minRange.Text = $"Минимальные расходы: {minValue:N2} руб. за {minPayment.accounting_date:dd.MM.yyyy}";
                            minParagraph.set_Style("Заголовок 3");
                            minRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                            minRange.InsertParagraphAfter();
                        }

                        document.Paragraphs.Add();
                    }

                    if (user != allUsers.LastOrDefault())
                        document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }

                wordApp.Visible = true;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Финансовый_отчет_{DateTime.Now:yyyyMMdd_HHmmss}";
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

        private decimal GetCategorySum(List<Accounting> finances, string category)
        {
            switch (category)
            {
                case "Зарплата": return finances.Sum(f => GetDecimalValue(f.salary));
                case "Коммунальные": return finances.Sum(f => GetDecimalValue(f.utilities));
                case "Налоги": return finances.Sum(f => GetDecimalValue(f.taxes));
                case "Лекарства": return finances.Sum(f => GetDecimalValue(f.medicine_expenses));
                case "Еда": return finances.Sum(f => GetDecimalValue(f.food_expenses));
                case "Прочие": return finances.Sum(f => GetDecimalValue(f.other_expenses));
                case "Взносы": return finances.Sum(f => GetDecimalValue(f.voluntary_contributions));
                default: return 0m;
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
                worksheet.Cells[1, 1] = "ID пользователя";
                worksheet.Cells[1, 2] = "Дата";
                worksheet.Cells[1, 3] = "Зарплата";
                worksheet.Cells[1, 4] = "Коммунальные";
                worksheet.Cells[1, 5] = "Налоги";
                worksheet.Cells[1, 6] = "Лекарства";
                worksheet.Cells[1, 7] = "Еда";
                worksheet.Cells[1, 8] = "Прочие";
                worksheet.Cells[1, 9] = "Взносы";
                worksheet.Cells[1, 10] = "Итого";

                // Данные
                var allData = Entities.GetContext().Accounting.ToList();

                int row = 2;
                foreach (var item in allData)
                {
                    worksheet.Cells[row, 1] = item.id;
                    worksheet.Cells[row, 2] = item.accounting_date.ToString("dd.MM.yyyy");
                    worksheet.Cells[row, 3] = GetDecimalValue(item.salary);
                    worksheet.Cells[row, 4] = GetDecimalValue(item.utilities);
                    worksheet.Cells[row, 5] = GetDecimalValue(item.taxes);
                    worksheet.Cells[row, 6] = GetDecimalValue(item.medicine_expenses);
                    worksheet.Cells[row, 7] = GetDecimalValue(item.food_expenses);
                    worksheet.Cells[row, 8] = GetDecimalValue(item.other_expenses);
                    worksheet.Cells[row, 9] = GetDecimalValue(item.voluntary_contributions);
                    worksheet.Cells[row, 10] = GetDecimalValue(item.total);
                    row++;
                }

                // Форматирование
                Excel.Range headerRange = worksheet.Range["A1:J1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                // Автоширина столбцов
                worksheet.Columns.AutoFit();

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = Path.Combine(desktopPath, $"Финансовый_отчет_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
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

        // Остальные методы без изменений
        private void SearchFinance_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFinances();
        }

        private void SortFinance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFinances();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchFinance.Text = "";
            SortFinance.SelectedIndex = -1;
            UpdateFinances();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFinance((sender as Button).DataContext as Accounting));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPageFinance(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var financesForRemoving = DataGridFinance.SelectedItems.Cast<Accounting>().ToList();
            if (financesForRemoving.Count == 0)
            {
                MessageBox.Show("Выберите записи для удаления", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить {financesForRemoving.Count} записей?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Accounting.RemoveRange(financesForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateFinances();
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
            NavigationService?.Navigate(new VacinationPage(_currentUser, _userRole));
        }

        private void LastButton_Cick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new NeedPage(_currentUser, _userRole));
        }

        private void FinancePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                UpdateFinances();

                if (CmbUser.SelectedItem != null)
                {
                    UpdateChart(null, null);
                }
            }
        }
    }
}
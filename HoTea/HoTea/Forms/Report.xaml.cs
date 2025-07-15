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



using System.IO;


using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;                   // Excel: ClosedXML
using Xceed.Words.NET;                     // Word: DocX (Xceed.Words.NET)
using iTextSharp.text;                     // PDF: iTextSharp
using iTextSharp.text.pdf;
using System.Windows.Forms;
using Xceed.Document.NET;


namespace lab9.Forms
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    /// 

    public partial class Report : Window
    {
        public Report()
        {
            InitializeComponent();
           
        }
        private (DateTime startDate, DateTime endDate) GetReportDateRange()
        {
            // Если у вас DateTimePicker:
            DateTime startDate = DateTime.Parse(startDatePicker.Text);
            DateTime endDate = DateTime.Parse(endDatePicker.Text);

            return (startDate, endDate);
        }


        // Унифицированная функция, которая открывает диалог сохранения и вызывает экспорт в нужный формат.
        public void GenerateSalesReportUnified()
        {
            var (startDate, endDate) = GetReportDateRange();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Сохранить отчет";
                sfd.Filter = "Excel файлы (*.xlsx)|*.xlsx|Word файлы (*.docx)|*.docx|PDF файлы (*.pdf)|*.pdf";
                sfd.FileName = "SalesReport";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filePath = sfd.FileName;
                    string ext = Path.GetExtension(filePath).ToLower();
                    switch (ext)
                    {
                        case ".xlsx":
                            GenerateExcelReport(startDate, endDate, filePath);
                            break;
                        case ".docx":
                            GenerateWordReport(startDate, endDate, filePath);
                            break;
                        case ".pdf":
                            GeneratePdfReport(startDate, endDate, filePath);
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("Неверный формат файла.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Сохранение отчета отменено.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Функция экспорта в Excel, использующая ClosedXML.
        private void GenerateExcelReport(DateTime startDate, DateTime endDate, string filePath)
        {
            using (var context = new AppDbContext())
            {
                // Запрос данных: объединение таблиц Заказы, ТоварыВЗаказе и Чай,
                // с фильтрацией по периоду и агрегацией по названию чая.
                var salesQuery = from order in context.Заказы
                                 where order.ДатаЗаказа >= startDate && order.ДатаЗаказа <= endDate
                                 join orderItem in context.ТоварыВЗаказе on order.КодЗаказа equals orderItem.КодЗаказа
                                 join tea in context.Чай on orderItem.КодЧая equals tea.КодЧая
                                 group orderItem by tea.Название into g
                                 select new
                                 {
                                     TeaName = g.Key,
                                     TotalQuantity = g.Sum(x => x.Количество),
                                     TotalSum = g.Sum(x => x.Сумма)
                                 };

                var salesData = salesQuery.OrderByDescending(x => x.TotalQuantity).ToList();
                int overallQuantity = salesData.Sum(x => x.TotalQuantity);
                decimal overallSum = salesData.Sum(x => x.TotalSum);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Отчет по продажам");

                    // Заголовок отчёта – объединённая ячейка с названием.
                    worksheet.Range("A1:C1").Merge().Value = "Отчет по продажам";
                    worksheet.Range("A1:C1").Style.Font.Bold = true;
                    worksheet.Range("A1:C1").Style.Font.FontSize = 16;
                    worksheet.Range("A1:C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Добавляем строку с информацией о периоде отчёта.
                    worksheet.Range("A2:C2").Merge().Value = $"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                    worksheet.Range("A2:C2").Style.Font.FontSize = 12;
                    worksheet.Range("A2:C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Заголовки колонок таблицы начинаем с 4-й строки.
                    worksheet.Cell("A4").Value = "Название чая";
                    worksheet.Cell("B4").Value = "Количество продаж";
                    worksheet.Cell("C4").Value = "Сумма продаж";

                    int currentRow = 5;
                    foreach (var item in salesData)
                    {
                        worksheet.Cell(currentRow, 1).Value = item.TeaName;
                        worksheet.Cell(currentRow, 2).Value = item.TotalQuantity;
                        worksheet.Cell(currentRow, 3).Value = item.TotalSum;
                        currentRow++;
                    }

                    // Итоговая строка с общими суммами.
                    worksheet.Cell(currentRow, 1).Value = "Итого";
                    worksheet.Cell(currentRow, 2).Value = overallQuantity;
                    worksheet.Cell(currentRow, 3).Value = overallSum;

                    // Автонастройка ширины колонок.
                    worksheet.Columns().AdjustToContents();

                    workbook.SaveAs(filePath);
                }
            }
            System.Windows.Forms.MessageBox.Show("Excel-отчет сохранен: " + filePath, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Функция экспорта в Word, использующая DocX.
        private void GenerateWordReport(DateTime startDate, DateTime endDate, string filePath)
        {
            using (var context = new AppDbContext())
            {
                var salesQuery = from order in context.Заказы
                                 where order.ДатаЗаказа >= startDate && order.ДатаЗаказа <= endDate
                                 join orderItem in context.ТоварыВЗаказе on order.КодЗаказа equals orderItem.КодЗаказа
                                 join tea in context.Чай on orderItem.КодЧая equals tea.КодЧая
                                 group orderItem by tea.Название into g
                                 select new
                                 {
                                     TeaName = g.Key,
                                     TotalQuantity = g.Sum(x => x.Количество),
                                     TotalSum = g.Sum(x => x.Сумма)
                                 };

                var salesData = salesQuery.OrderByDescending(x => x.TotalQuantity).ToList();
                int overallQuantity = salesData.Sum(x => x.TotalQuantity);
                decimal overallSum = salesData.Sum(x => x.TotalSum);

                var doc = DocX.Create(filePath);

                // Заголовок и строка с периодом.
                doc.InsertParagraph("Отчет по продажам")
                   .Bold().FontSize(16)
                   .Alignment = Xceed.Document.NET.Alignment.center;
                doc.InsertParagraph($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}")
                   .FontSize(12).SpacingAfter(10)
                   .Alignment = Xceed.Document.NET.Alignment.center;

                // Создаем таблицу: заголовок, строки данных и итоговая строка.
                int rowsCount = salesData.Count + 2; // 1 строка заголовка + n строк данных + итоговая строка
                int columnsCount = 3;
                var table = doc.AddTable(rowsCount, columnsCount);
                table.Design = TableDesign.MediumList2Accent1;

                table.Rows[0].Cells[0].Paragraphs[0].Append("Название чая").Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("Количество продаж").Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("Сумма продаж").Bold();

                int currentRow = 1;
                foreach (var item in salesData)
                {
                    table.Rows[currentRow].Cells[0].Paragraphs[0].Append(item.TeaName);
                    table.Rows[currentRow].Cells[1].Paragraphs[0].Append(item.TotalQuantity.ToString());
                    table.Rows[currentRow].Cells[2].Paragraphs[0].Append(item.TotalSum.ToString("F2"));
                    currentRow++;
                }
                table.Rows[currentRow].Cells[0].Paragraphs[0].Append("Итого").Bold();
                table.Rows[currentRow].Cells[1].Paragraphs[0].Append(overallQuantity.ToString()).Bold();
                table.Rows[currentRow].Cells[2].Paragraphs[0].Append(overallSum.ToString("F2")).Bold();

                doc.InsertTable(table);
                doc.Save();
            }
            System.Windows.Forms.MessageBox.Show("Word-отчет сохранен: " + filePath, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Функция экспорта в PDF, использующая iTextSharp.
        private void GeneratePdfReport(DateTime startDate, DateTime endDate, string filePath)
        {
            using (var context = new AppDbContext())
            {
                var salesQuery = from order in context.Заказы
                                 where order.ДатаЗаказа >= startDate && order.ДатаЗаказа <= endDate
                                 join orderItem in context.ТоварыВЗаказе on order.КодЗаказа equals orderItem.КодЗаказа
                                 join tea in context.Чай on orderItem.КодЧая equals tea.КодЧая
                                 group orderItem by tea.Название into g
                                 select new
                                 {
                                     TeaName = g.Key.ToString(),
                                     TotalQuantity = g.Sum(x => x.Количество),
                                     TotalSum = g.Sum(x => x.Сумма)
                                 };

                var salesData = salesQuery.OrderByDescending(x => x.TotalQuantity).ToList();
                int overallQuantity = salesData.Sum(x => x.TotalQuantity);
                decimal overallSum = salesData.Sum(x => x.TotalSum);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    // Создаем PDF-документ с отступами
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 20, 20, 20, 20);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Создаем BaseFont с поддержкой кириллицы (используем Arial с Identity-H)
                    BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font bodyFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);

                    // Заголовок отчета
                    iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Отчет по продажам", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDoc.Add(title);

                    iTextSharp.text.Paragraph periodParagraph = new iTextSharp.text.Paragraph($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}", bodyFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 10f
                    };
                    pdfDoc.Add(periodParagraph);

                    // Создаем таблицу с 3 столбцами; увеличиваем первую колонку
                    PdfPTable table = new PdfPTable(3)
                    {
                        WidthPercentage = 100
                    };
                    table.SetWidths(new float[] { 8, 2, 2 });

                    // Заголовки столбцов
                    PdfPCell headerCell = new PdfPCell(new Phrase("Название чая", headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        NoWrap = false,
                        Padding = 5f
                    };
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Количество продаж", headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5f
                    };
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Сумма продаж", headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5f
                    };
                    table.AddCell(headerCell);

                    // Заполнение данных
                    foreach (var item in salesData)
                    {
                        PdfPCell nameCell = new PdfPCell(new Phrase(item.TeaName, bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            NoWrap = false,
                            Padding = 5f
                        };
                        table.AddCell(nameCell);

                        PdfPCell qtyCell = new PdfPCell(new Phrase(item.TotalQuantity.ToString(), bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            Padding = 5f
                        };
                        table.AddCell(qtyCell);

                        PdfPCell sumCell = new PdfPCell(new Phrase(item.TotalSum.ToString("F2"), bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            Padding = 5f
                        };
                        table.AddCell(sumCell);
                    }

                    // Итоговая строка
                    PdfPCell totalCell = new PdfPCell(new Phrase("Итого", headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5f
                    };
                    table.AddCell(totalCell);

                    totalCell = new PdfPCell(new Phrase(overallQuantity.ToString(), headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Padding = 5f
                    };
                    table.AddCell(totalCell);

                    totalCell = new PdfPCell(new Phrase(overallSum.ToString("F2"), headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Padding = 5f
                    };
                    table.AddCell(totalCell);

                    pdfDoc.Add(table);
                    pdfDoc.Close();
                    writer.Close();
                }
            }
            System.Windows.Forms.MessageBox.Show("PDF-отчет сохранен: " + filePath, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }






        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (startDatePicker.Text == string.Empty || endDatePicker.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("Веберете периоды", "Ошибка");
                return;
            }
            var (startDate, endDate) = GetReportDateRange();
            GenerateSalesReportUnified();

        }   
    }
}

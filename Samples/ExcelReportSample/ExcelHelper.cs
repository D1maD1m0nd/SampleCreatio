using System.Text.RegularExpressions;
using System.Xml;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using OfficeOpenXml;

namespace Terrasoft.Configuration.HaleonReport.Helper
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Collections.Generic;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    public static class HaleonReportExcelHelper
    {
        public const int _initCapacity = 100_000;

        public static List<Dictionary<string, object>> readXLS(string FilePath)
        {
            var list = new List<Dictionary<string, object>>();
            FileInfo existingFile = new FileInfo(FilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                int colCount = worksheet.Dimension.End.Column; //get Column Count
                int rowCount = worksheet.Dimension.End.Row; //get row count
                var headerCol = new List<string>(colCount);
                for (int row = 1; row <= rowCount; row++)
                {
                    var dict = new Dictionary<string, object>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (row == 1)
                        {
                            headerCol.Add((string)worksheet.Cells[row, col].Value);
                            continue;
                        }

                        object cellValue = worksheet.Cells[row, col].Value;
                        string cellType = cellValue == null ? "NULL" : cellValue.GetType().ToString();
                        if (cellValue.GetType() != null)
                        {
                            dict.Add(headerCol[col - 1], cellValue);
                        }
                    }

                    list.Add(dict);
                }
            }

            return list;
        }

        public static List<Dictionary<string, string>> GetExcelData(string flePath)
        {
            var file = File.ReadAllBytes(@"C:\Users\d1m0h\OneDrive\Рабочий стол\Supply_plan_01.05_upload_2500_записей.xlsx");
            using (MemoryStream stream = new MemoryStream(file))
            {
                return GetExcelData(stream);
            }
        }

        private static List<Dictionary<string, string>> GetExcelData(Stream fileStream)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
                    Worksheet worksheet = worksheetPart.Worksheet;
                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    SharedStringTable sharedStringTable = sstpart.SharedStringTable;

                    // Assume the first row is the header
                    Row headerRow = worksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault();

                    List<string> headers = new List<string>();
                    foreach (Cell headerCell in headerRow.Elements<Cell>())
                    {
                        headers.Add(GetCellValue(headerCell, sharedStringTable, string.Empty));
                    }

                    foreach (Row row in worksheet.GetFirstChild<SheetData>().Elements<Row>().Skip(1))
                    {
                        var rowData = new Dictionary<string, string>();

                        int cellIndex = 0;
                        foreach (Cell cell in row.Elements<Cell>())
                        {
                            while (GetColumnIndexFromName(GetColumnName(cell.CellReference)) > cellIndex)
                            {
                                rowData[headers[cellIndex]] = string.Empty;
                                cellIndex += 1;
                            }

                            rowData[headers[cellIndex]] = GetCellValue(cell, sharedStringTable, headers[cellIndex]);
                            cellIndex += 1;
                        }

                        data.Add(rowData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading the Excel data", ex);
            }

            return data;
        }


        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value.ToUpper();
        }

        private static int GetColumnIndexFromName(string columnName)
        {
            int columnIndex = 0;
            int factor = 1;
            for (int pos = columnName.Length - 1; pos >= 0; pos--)
            {
                columnIndex += (columnName[pos] - 'A' + 1) * factor;
                factor *= 26;
            }

            // Convert to 0-based index
            return columnIndex - 1;
        }


        private static string GetCellValue(Cell cell, SharedStringTable sharedStringTable, string header)
            {
                string value = cell.InnerText;

                if (cell.DataType != null)
                {
                    if (cell.DataType.Value == CellValues.SharedString)
                    {
                        int sstIndex = int.Parse(value);
                        value = sharedStringTable.ChildElements[sstIndex].InnerText;
                    }
                    else if (cell.DataType.Value == CellValues.Boolean)
                    {
                        value = value == "0" ? "FALSE" : "TRUE";
                    }
                    else if (cell.DataType.Value == CellValues.Date)
                    {
                        value = ConvertDate(value);
                    }
                }
                else if (header.ToLower().Contains("date") || header.ToLower().Contains("version"))
                {
                    value = ConvertDate(value);
                }

                return value;
            }

            private static string ConvertDate(string value)
            {
                double dateInDouble;
                if (double.TryParse(value, out dateInDouble))
                {
                    DateTime dateValue = DateTime.FromOADate(dateInDouble);
                    return dateValue.ToString("dd.MM.yyyy");
                }

                return value;
            }
        }
    }
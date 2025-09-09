using BarcodePrintLabel.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BarcodePrintLabel.Core.Services
{

    public static class ExcelHelper
    {
        public const string sheetName = "Results";

        public static bool LoadFromExcel(string filePath, ref List<TestResult> results)
        {
            results = new List<TestResult>();

            try
            {
                if (!File.Exists(filePath))
                {
                    // Tạo file mới với sheet "Results" và header
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add(sheetName);

                        // tạo header (nếu cần)
                        AddDataToRow(ws, 1, new TestResult());
                        
                        workbook.SaveAs(filePath);
                    }

                    return true; // file mới tạo, chưa có dữ liệu
                }

                // Nếu file tồn tại thì đọc dữ liệu
                string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
                File.Copy(filePath, tempPath, true);

                using (var workbook = new XLWorkbook(tempPath))
                {
                    var ws = workbook.Worksheet(sheetName);
                    var rows = ws.RangeUsed()?.RowsUsed().Skip(1); // bỏ header

                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            var index = 1;
                            results.Add(new TestResult
                            {
                                SerialNumber = row.Cell(1).GetString(),
                                QRCode = row.Cell(2).GetString(),
                                LS_HORIZONTAL = row.Cell(3).GetString(),
                                LS_VERTICAL = row.Cell(4).GetString(),
                                LS_HORIZONTAL2 = row.Cell(5).GetString(),
                                LS_VERTICAL2 = row.Cell(6).GetString(),
                                RS_HORIZONTAL = row.Cell(7).GetString(),
                                RS_VERTICAL = row.Cell(8).GetString(),
                                RS_HORIZONTAL2 = row.Cell(9).GetString(),
                                RS_VERTICAL2 = row.Cell(10).GetString(),
                                RESULT = row.Cell(11).GetString(),
                                ERROR_CODE = row.Cell(12).GetString(),
                                DateTime = row.Cell(13).GetDateTime(),
                            });
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void SaveToExcel(string filePath, List<TestResult> results)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add(sheetName);

                // Header
                AddDataToRow(ws, 1, new TestResult());
                // Data
                var row = 2;
                foreach (var r in results)
                {
                    AddDataToRow(ws, row++, r);
                }

                workbook.SaveAs(filePath);
            }
        }

        private static void AddDataToRow(IXLWorksheet ws, int row, TestResult newResult)
        {
            var index = 1;

            var isFirstRow = row == 1;

            ws.Cell(row, index++).Value = isFirstRow ? "Serial Number" : newResult.SerialNumber;
            ws.Cell(row, index++).Value = isFirstRow ? "QR Code" : newResult.QRCode;
            ws.Cell(row, index++).Value = isFirstRow ? "LS_HORIZONTAL": newResult.LS_HORIZONTAL;
            ws.Cell(row, index++).Value = isFirstRow ? "LS_VERTICAL": newResult.LS_VERTICAL;
            ws.Cell(row, index++).Value = isFirstRow ? "LS_HORIZONTAL2" : newResult.LS_HORIZONTAL2;
            ws.Cell(row, index++).Value = isFirstRow ? "LS_VERTICAL2" : newResult.LS_VERTICAL2;
            ws.Cell(row, index++).Value = isFirstRow ? "RS_HORIZONTAL": newResult.RS_HORIZONTAL;
            ws.Cell(row, index++).Value = isFirstRow ? "RS_VERTICAL":  newResult.RS_VERTICAL;
            ws.Cell(row, index++).Value = isFirstRow ? "RS_HORIZONTAL2" : newResult.RS_HORIZONTAL2;
            ws.Cell(row, index++).Value = isFirstRow ? "RS_VERTICAL2" : newResult.RS_VERTICAL2;
            ws.Cell(row, index++).Value = isFirstRow ? "RESULT" : newResult.RESULT;
            ws.Cell(row, index++).Value = isFirstRow ? "ERROR_CODE" : newResult.ERROR_CODE;
            ws.Cell(row, index++).Value = isFirstRow ? "Date Time" : newResult.DateTime;
        }

        public static void AppendToExcel(string filePath, TestResult newResult)
        {

                // Nếu file chưa tồn tại thì tạo mới
                if (!File.Exists(filePath))
             {
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add(sheetName);

                    // Tạo header
                    AddDataToRow(ws, 1, new TestResult());
                    // Ghi dòng dữ liệu đầu tiên
                    AddDataToRow(ws, 2, newResult);

                    workbook.SaveAs(filePath);
                }
            }
            else
            {
                if (IsFileLocked(filePath))
                {
                    // Cách 1: Báo user
                    MessageBox.Show("File Excel đang mở, vui lòng đóng lại rồi thử lại.", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Cách 2 (tuỳ bạn): lưu sang file khác
                    // string backupPath = Path.Combine(Path.GetDirectoryName(filePath)!,
                    //     Path.GetFileNameWithoutExtension(filePath) + "_backup.xlsx");
                    // workbook.SaveAs(backupPath);
                    return;
                }
                // Nếu file đã có → mở ra và thêm dòng mới
                using (var workbook = new XLWorkbook(filePath))
                {
                    var ws = workbook.Worksheet(sheetName);

                    // Tìm dòng cuối cùng có dữ liệu
                    var lastRow = ws.LastRowUsed().RowNumber();
                        // Ghi dữ liệu vào dòng kế tiếp
                    AddDataToRow(ws, lastRow + 1, newResult);
                    workbook.Save();
                }
            }
        }

        private static bool IsFileLocked(string path)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true; // file bị lock
            }
            finally
            {
                stream?.Close();
            }
        }
    }
}

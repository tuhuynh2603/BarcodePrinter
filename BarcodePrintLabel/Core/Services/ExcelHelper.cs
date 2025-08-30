using BarcodePrintLabel.Models;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BarcodePrintLabel.Core.Services
{

    public static class ExcelHelper
    {
        public static List<TestResult> LoadFromExcel(string filePath)
        {
            var results = new List<TestResult>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var ws = workbook.Worksheet("Results");
                var rows = ws.RangeUsed().RowsUsed().Skip(1); // bỏ header

                foreach (var row in rows)
                {
                    results.Add(new TestResult
                    {
                        DateTime = row.Cell(1).GetString(),
                        Mode = row.Cell(2).GetString(),
                        RS_HM = row.Cell(3).GetString(),
                        RS_VM = row.Cell(4).GetString(),
                        LS_HM = row.Cell(5).GetString(),
                        LS_VM = row.Cell(6).GetString(),
                        Overall = row.Cell(7).GetString(),
                        ErrorCode = row.Cell(8).GetString(),
                        SerialNumber = row.Cell(9).GetString(),
                        QRCode = row.Cell(10).GetString()
                    });
                }
            }

            return results;
        }

        public static void SaveToExcel(string filePath, List<TestResult> results)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Results");

                // Header
                ws.Cell(1, 1).Value = "Timestamp";
                ws.Cell(1, 2).Value = "Mode";
                ws.Cell(1, 3).Value = "RS-HM";
                ws.Cell(1, 4).Value = "RS-VM Result";
                ws.Cell(1, 5).Value = "LS-HM";
                ws.Cell(1, 6).Value = "LS-VM";
                ws.Cell(1, 7).Value = "Overall";
                ws.Cell(1, 8).Value = "Error Code";
                ws.Cell(1, 9).Value = "Serial Number";
                ws.Cell(1, 10).Value = "TCE QR";

                // Data
                int row = 2;
                foreach (var r in results)
                {
                    ws.Cell(row, 1).Value = r.DateTime;
                    ws.Cell(row, 2).Value = r.Mode;
                    ws.Cell(row, 3).Value = r.RS_HM;
                    ws.Cell(row, 4).Value = r.RS_VM;
                    ws.Cell(row, 5).Value = r.LS_HM;
                    ws.Cell(row, 6).Value = r.LS_VM;
                    ws.Cell(row, 7).Value = r.Overall;
                    ws.Cell(row, 8).Value = r.ErrorCode;
                    ws.Cell(row, 9).Value = r.SerialNumber;
                    ws.Cell(row, 10).Value = r.QRCode;
                    row++;
                }

                workbook.SaveAs(filePath);
            }
        }

    public static void AppendToExcel(string filePath, TestResult newResult)
    {
            // Nếu file chưa tồn tại thì tạo mới
        if (!File.Exists(filePath))
         {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Results");

                // Tạo header
                ws.Cell(1, 1).Value = "Timestamp";
                ws.Cell(1, 2).Value = "Mode";
                ws.Cell(1, 3).Value = "RS-HM";
                ws.Cell(1, 4).Value = "RS-VM Result";
                ws.Cell(1, 5).Value = "LS-HM";
                ws.Cell(1, 6).Value = "LS-VM";
                ws.Cell(1, 7).Value = "Overall";
                ws.Cell(1, 8).Value = "Error Code";
                ws.Cell(1, 9).Value = "Serial Number";
                ws.Cell(1, 10).Value = "TCE QR";

                // Ghi dòng dữ liệu đầu tiên
                ws.Cell(2, 1).Value = newResult.DateTime;
                ws.Cell(2, 2).Value = newResult.Mode;
                ws.Cell(2, 3).Value = newResult.RS_HM;
                ws.Cell(2, 4).Value = newResult.RS_VM;
                ws.Cell(2, 5).Value = newResult.LS_HM;
                ws.Cell(2, 6).Value = newResult.LS_VM;
                ws.Cell(2, 7).Value = newResult.Overall;
                ws.Cell(2, 8).Value = newResult.ErrorCode;
                ws.Cell(2, 9).Value = newResult.SerialNumber;
                ws.Cell(2, 10).Value = newResult.QRCode;

                workbook.SaveAs(filePath);
            }
        }
        else
        {
            // Nếu file đã có → mở ra và thêm dòng mới
            using (var workbook = new XLWorkbook(filePath))
            {
                var ws = workbook.Worksheet("Results");

                // Tìm dòng cuối cùng có dữ liệu
                int lastRow = ws.LastRowUsed().RowNumber();

                // Ghi dữ liệu vào dòng kế tiếp
                int newRow = lastRow + 1;
                ws.Cell(newRow, 1).Value = newResult.DateTime;
                ws.Cell(newRow, 2).Value = newResult.Mode;
                ws.Cell(newRow, 3).Value = newResult.RS_HM;
                ws.Cell(newRow, 4).Value = newResult.RS_VM;
                ws.Cell(newRow, 5).Value = newResult.LS_HM;
                ws.Cell(newRow, 6).Value = newResult.LS_VM;
                ws.Cell(newRow, 7).Value = newResult.Overall;
                ws.Cell(newRow, 8).Value = newResult.ErrorCode;
                ws.Cell(newRow, 9).Value = newResult.SerialNumber;
                ws.Cell(newRow, 10).Value = newResult.QRCode;

                workbook.Save();
            }
        }
    }
}
}

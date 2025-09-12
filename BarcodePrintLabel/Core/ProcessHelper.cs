using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Core
{
    public static class ProcessHelper
    {
     
        public static void KillCurrentProcess()
        {
            Environment.Exit(0);
        }
        public static bool CheckMuTexProcess()
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                return false;
            }
            else
            { return true; }
        }

        public static string GetWeekOfYear(DateTime date)
        {
            CultureInfo cul = CultureInfo.InvariantCulture;
            CalendarWeekRule weekRule = cul.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = cul.DateTimeFormat.FirstDayOfWeek;

            // Lấy số tuần
            return cul.Calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek).ToString("D2");
        }
    }
}

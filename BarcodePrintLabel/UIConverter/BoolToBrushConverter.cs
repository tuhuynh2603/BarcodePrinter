using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BarcodePrintLabel.UIConverter
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush TrueBrush { get; set; } = Brushes.Green; // màu khi true
        public Brush FalseBrush { get; set; } = Brushes.Red;   // màu khi false
        public bool Invert { get; set; } = false;              // đảo ngược nếu cần

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (Invert) b = !b;
                return b ? TrueBrush : FalseBrush;
            }
            // giá trị null/không phải bool → trả về FalseBrush
            return FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing; // không dùng chiều ngược
    }
}


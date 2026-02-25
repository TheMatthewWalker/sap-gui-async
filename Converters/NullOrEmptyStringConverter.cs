using Microsoft.UI.Xaml.Data;
using System;

namespace costing_tool.Converters
{
    public class NullOrEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //if (value == null) return " ";
            var str = value.ToString();
            return string.IsNullOrWhiteSpace(str) ? " " : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //if (value == null) return "";
            var str = value.ToString();
            return string.IsNullOrWhiteSpace(str) ? "" : str;
        }
    }
}
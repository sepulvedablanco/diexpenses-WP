namespace diexpenses.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class BooleanToToggleButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Boolean)
            {
                return (bool)value ? "Expense" : "Income";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return ((string)value) == "Expense";
            }
            return value;
        }
    }
}

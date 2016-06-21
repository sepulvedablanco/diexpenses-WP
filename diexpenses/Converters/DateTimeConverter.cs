namespace diexpenses.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.Parse(value.ToString());
        }
    }
}

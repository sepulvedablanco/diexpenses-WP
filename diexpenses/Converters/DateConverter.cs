namespace diexpenses.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }

            if (value is DateTime)
            {
                var dateTime = (DateTime)value;
                return dateTime.ToString("dd/MM/yyyy");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

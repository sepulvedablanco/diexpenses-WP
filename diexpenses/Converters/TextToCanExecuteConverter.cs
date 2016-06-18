namespace diexpenses.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class TextToCanExecuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return false;

            return value.ToString().Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

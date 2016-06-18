namespace diexpenses.Converters
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml.Data;

    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "0.00€";
            }

            return String.Format(CultureInfo.InvariantCulture, "{0:0,0.##}", value) + "€";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

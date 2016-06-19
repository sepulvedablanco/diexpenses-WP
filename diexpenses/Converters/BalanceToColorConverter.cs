namespace diexpenses.Converters
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml.Data;

    public class BalanceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "Black";
            }

            var number = double.Parse(value.ToString());

            if (number == 0)
            {
                return "Black";
            }

            return number > 0 ? "Green" : "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

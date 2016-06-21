namespace diexpenses.Converters
{
    using Common;
    using System;
    using Windows.UI.Xaml.Data;

    public class MovementToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "Black";
            }

            return ((bool)value ? "Red" : "Green");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

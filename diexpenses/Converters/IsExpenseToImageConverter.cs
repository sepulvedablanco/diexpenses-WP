namespace diexpenses.Converters
{
    using Common;
    using System;
    using Windows.UI.Xaml.Data;

    public class IsExpenseToImageConverter : IValueConverter
    {
        public readonly static string MOVEMENTS_DIRECTORY = "ms-appx:///Images/Movements/";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Constants.COMMON_IMAGES_DIRECTORY + "unknown" + Constants.IMAGES_FORMAT;
            }

            return MOVEMENTS_DIRECTORY + ((bool)value ? "expense" : "income") + Constants.IMAGES_FORMAT;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

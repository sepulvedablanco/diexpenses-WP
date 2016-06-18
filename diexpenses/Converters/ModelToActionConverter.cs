namespace diexpenses.Converters
{
    using Entities;
    using System;
    using Windows.UI.Xaml.Data;

    public class ModelToActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || (value as BankAccount).Id == null)
            {
                return "Save";
            }

            return "Update";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

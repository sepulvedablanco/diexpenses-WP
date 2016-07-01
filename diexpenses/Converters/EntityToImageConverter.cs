namespace diexpenses.Converters
{
    using common.Common;
    using System;
    using System.Collections.Generic;
    using Windows.UI.Xaml.Data;

    public class EntityToImageConverter : IValueConverter
    {
        public readonly static List<string> ENTITIES = new List<string> { "0019", "0049", "0073", "0075", "0128", "0131", "0182", "0216", "0239",
                                                                          "1465", "2038", "2048", "2085", "2095", "2100", "2108" ,"3058" };

        public readonly static string BANK_ENTITIES_DIRECTORY = "ms-appx:///Images/BankEntities/";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !ENTITIES.Contains(value.ToString()))
            {
                return Constants.COMMON_IMAGES_DIRECTORY + "unknown" + Constants.IMAGES_FORMAT;
            }
            
            return BANK_ENTITIES_DIRECTORY + value.ToString() + Constants.IMAGES_FORMAT;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

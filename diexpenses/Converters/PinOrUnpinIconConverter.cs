namespace diexpenses.Converters
{
    using Common;
    using System;
    using Windows.UI.StartScreen;
    using Windows.UI.Xaml.Data;

    public class PinOrUnpinIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string tileId = Utils.GetTileId();
            if (tileId == null || !SecondaryTile.Exists(tileId))
            {
                return "Pin";
            } else
            {
                return "UnPin";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

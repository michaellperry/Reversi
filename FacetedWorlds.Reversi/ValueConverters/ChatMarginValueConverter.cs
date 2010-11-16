using System;
using System.Windows;
using System.Windows.Data;

namespace FacetedWorlds.Reversi.ValueConverters
{
    public class ChatMarginValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Thickness))
            {
                bool sourceValue = (bool)value;
                return sourceValue
                    ? new Thickness(120, 0, 0, 0)
                    : new Thickness(8, 0, 0, 0);
            }
            else
                throw new ArgumentException(String.Format("Can't convert to type {0}.", targetType));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FacetedWorlds.Reversi.ValueConverters
{
    public class VisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                bool sourceValue = value is bool
                    ? (bool)value
                    : value != null;
                return sourceValue
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                throw new ArgumentException(String.Format("Can't convert to type {0}.", targetType));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

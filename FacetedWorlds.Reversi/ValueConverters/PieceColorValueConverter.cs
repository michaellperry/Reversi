using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ValueConverters
{
    public class PieceGlossValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PieceColor pieceColor = (PieceColor)value;
            if (targetType == typeof(Brush))
            {
                return App.Current.Resources[
                    pieceColor == PieceColor.Black ? "BlackGlossBrush" :
                    pieceColor == PieceColor.White ? "WhiteGlossBrush" :
                        "TransparentBrush"];

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

	public class PieceColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PieceColor pieceColor = (PieceColor)value;
            if (targetType == typeof(Brush))
            {
                return App.Current.Resources[
                    pieceColor == PieceColor.Black ? "BlackPieceBrush" :
                    pieceColor == PieceColor.White ? "WhitePieceBrush" :
                        "TransparentBrush"];

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

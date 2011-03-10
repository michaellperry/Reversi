using System.Windows;

namespace FacetedWorlds.Reversi.Views
{
    public static class PointExtensions
    {
        public static Point Minus(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point Plus(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}

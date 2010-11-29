using System.Windows;
using System.Windows.Media;

namespace FacetedWorlds.Reversi
{
    public static class VisualTreeExtensions
    {
        public static T GetAncestorOfType<T>(this DependencyObject child)
                where T : DependencyObject
        {
            DependencyObject parent = child;

            while (parent != null)
                if (typeof(T).IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }
    }
}

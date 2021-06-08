using System.Windows;
using System.Windows.Media;

namespace Budgeter.WPFApplication.Helpers
{
    public static class ViewHelper
    {
        public static T FindAncestor<T>(DependencyObject source) where T : DependencyObject
        {
            var current = source;

            do
            {
                if (current is T t)
                {
                    return t;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);

            return null;
        }

        public static T GetChild<T>(DependencyObject source) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);

                if (child is T t)
                {
                    return t;
                }
                else
                {
                    var grandChild = GetChild<T>(child);

                    if (grandChild != null)
                    {
                        return grandChild;
                    }
                }
            }

            return null;
        }
    }
}

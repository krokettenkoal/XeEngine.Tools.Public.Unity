using System;
using System.Windows;
using System.Windows.Media;

namespace Xe.Tools.Wpf.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static T GetParent<T>(this FrameworkElement frameworkElement, string name)
            where T : FrameworkElement =>
            GetParent<T>(frameworkElement, x => x.Name == name);

        public static T GetParent<T>(this FrameworkElement frameworkElement, Func<T, bool> filter)
            where T : FrameworkElement
        {
            if (frameworkElement == null)
                return null;

            if (frameworkElement is T myFrameworkElement && filter(myFrameworkElement))
                return myFrameworkElement;

            if (!(VisualTreeHelper.GetParent(frameworkElement) is FrameworkElement parent))
                return null;

            return GetParent(parent, filter);
        }

        public static T GetControl<T>(this FrameworkElement frameworkElement, string name)
            where T : FrameworkElement =>
            GetControl<T>(frameworkElement, x => x.Name == name);

        public static T GetControl<T>(this FrameworkElement frameworkElement, Func<T, bool> filter)
            where T : FrameworkElement
        {
            if (frameworkElement == null)
                return null;

            if (frameworkElement is T myFrameworkElement && filter(myFrameworkElement))
                return myFrameworkElement;

            var parentChildCount = VisualTreeHelper.GetChildrenCount(frameworkElement);
            for (var i = 0; i < parentChildCount; i++)
            {
                var child = VisualTreeHelper.GetChild(frameworkElement, i);
                if (child is FrameworkElement childElement)
                {
                    var foundElement = GetControl(childElement, filter);
                    if (foundElement != null)
                        return foundElement as T;
                }
            }

            return null;
        }
    }
}

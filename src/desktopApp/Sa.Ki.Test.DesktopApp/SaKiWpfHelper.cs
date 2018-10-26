namespace Sa.Ki.Test.DesktopApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class SaKiWpfHelper
    {
        public static ChildItemT FindVisualChild<ChildItemT>(DependencyObject obj)
            where ChildItemT : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildItemT)
                    return (ChildItemT)child;
                else
                {
                    ChildItemT childOfChild = FindVisualChild<ChildItemT>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static TreeViewItem FindTreeViewItemForObject(ItemsControl itemsControl, object obj)
        {
            var generator = itemsControl.ItemContainerGenerator;

            var treeViewItem = generator.ContainerFromItem(obj) as TreeViewItem;
            if (treeViewItem != null)
                return treeViewItem;

            foreach (var subItem in itemsControl.Items)
            {
                treeViewItem = generator.ContainerFromItem(subItem) as TreeViewItem;
                if (treeViewItem == null) continue;


                var search = FindTreeViewItemForObject(treeViewItem, obj);
                if (search != null)
                    return search;
            }
            return null;
        }
    }
}

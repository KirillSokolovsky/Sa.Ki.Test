namespace Sa.Ki.Test.DesktopApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
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
                treeViewItem = subItem as TreeViewItem;
                if (treeViewItem == null)
                    treeViewItem = generator.ContainerFromItem(subItem) as TreeViewItem;

                if (treeViewItem == null)
                    continue;

                var isExp = treeViewItem.IsExpanded;
                if(!isExp)
                {
                    treeViewItem.IsExpanded = true;
                    treeViewItem.UpdateLayout();
                    treeViewItem.IsExpanded = false;
                }

                var search = FindTreeViewItemForObject(treeViewItem, obj);
                if (search != null)
                    return search;
            }
            return null;
        }

        public static void ExpandCollapseItemsControl(ItemsControl itemsControl)
        {
            if (itemsControl is TreeViewItem tvi)
            {
                var isExp = tvi.IsExpanded;
                if(!isExp)
                {
                    tvi.IsExpanded = true;
                    itemsControl.UpdateLayout();
                    tvi.IsExpanded = false;
                }
            }

            foreach (var item in itemsControl.Items)
            {
                tvi = itemsControl.ItemContainerGenerator.ContainerFromItem(item)
                    as TreeViewItem;
                if (tvi != null)
                    ExpandCollapseItemsControl(tvi);
            }
        }

        public static void RegenerateItems(ItemsControl itemsControl)
        {
            var items = itemsControl.Items;

            IItemContainerGenerator generator = itemsControl.ItemContainerGenerator;
            GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
            using (generator.StartAt(position, GeneratorDirection.Forward, true))
            {
                foreach (object o in items)
                {
                    DependencyObject dp = generator.GenerateNext();
                    generator.PrepareItemContainer(dp);
                }
            }

            var gen = itemsControl.ItemContainerGenerator;
            foreach (object o in items)
            {
                var treeViewItem = gen.ContainerFromItem(o) as TreeViewItem;
                treeViewItem.IsExpanded = true; treeViewItem.IsExpanded = true;
                if (treeViewItem != null)
                    RegenerateItems(treeViewItem);
            }
        }
    }
}

using System.Windows.Controls;
using System.Windows.Media;

namespace DarkBlendTheme
{
    /// <summary>
    /// Tree item extension
    /// </summary>
    public static class TreeViewItemExtensions
    {
        /// <summary>
        /// Get the depth of the item
        /// </summary>
        /// <param name="item">The tree item</param>
        /// <returns>The depth of the item</returns>
        public static int GetDepth(this TreeViewItem item)
        {
            TreeViewItem parent;
            while ((parent = GetParent(item)) != null)
            {
                return GetDepth(parent) + 1;
            }
            return 0;
        }

        /// <summary>
        /// Gets the parent of the item
        /// </summary>
        /// <param name="item">The item to get the parent of</param>
        /// <returns>The parent of the item</returns>
        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);

            while (!(parent is TreeViewItem || parent is TreeView))
            {
                if (parent == null) return null;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TreeViewItem;
        }
    }
}

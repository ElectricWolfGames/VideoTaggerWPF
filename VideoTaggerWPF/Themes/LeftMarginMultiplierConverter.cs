using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DarkBlendTheme
{
    /// <summary>
    /// Left Margin Multiplier Converter
    /// </summary>
    public class LeftMarginMultiplierConverter : IValueConverter
    {
        /// <summary>
        /// Gets the length
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// convert of the value
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Extra data</param>
        /// <param name="culture">The culture</param>
        /// <returns>The converted type</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TreeViewItem;
            if (item == null)
                return new Thickness(0);

            return new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }

        /// <summary>
        /// Convert back
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Extra data</param>
        /// <param name="culture">The culture</param>
        /// <returns>The converted type</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}

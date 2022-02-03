using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
    /// <summary>
    /// Returns true if this is the first item in an ItemsControl
    /// </summary>
    public class IsFirstConverter : ConverterMarkupExtension<IsFirstConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DependencyObject item = (DependencyObject)value;
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(item);

            return itemsControl.ItemContainerGenerator.IndexFromContainer(item) == 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns true if this is the last item in an ItemsControl
    /// </summary>
    public class IsLastConverter : ConverterMarkupExtension<IsLastConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DependencyObject item = (DependencyObject)value;
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(item);

            return itemsControl.ItemContainerGenerator.IndexFromContainer(item) == itemsControl.Items.Count - 1;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Value 0: ItemsPresenter
    /// Value 1: an item under ItemsPresenter
    /// Returns Item's index under ItemsPresenter
    /// </summary>
    public class ItemToIndexConverter : MultiConverterMarkupExtension<ItemToIndexConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values))
                return Binding.DoNothing;

            if (values[0] is ItemsControl itemsControl && values[1] is DependencyObject item)
                return itemsControl.ItemContainerGenerator.IndexFromContainer(item);

            return Binding.DoNothing;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

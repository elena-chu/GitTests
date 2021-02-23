using System;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    /// <summary>
    /// Class targeting the need to use the an absolute values like width, Height, Margin 
    /// that are independent from device unit size of Wpf(dependent on Display->TextSize).
    /// </summary>
    public class IndependentSize
    {
        // Double properties ************************************

        /// <summary>
        /// Independent Width property of FrameworkElement. Updates the original Width property
        /// </summary>
        public static DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnWidthPropertyChanged));

        public static void SetWidth(FrameworkElement obj, double value)
        {
            obj.SetValue(WidthProperty, value);
        }
        public static double GetWidth(FrameworkElement obj)
        {
            return (double)obj.GetValue(WidthProperty);
        }

        public static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.Width = GetProportionalDoubleProperty(e);
        }

        /// <summary>
        /// Independent MinWidth property of FrameworkElement. Updates the original MinWidth property
        /// </summary>
        public static DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMinWidthPropertyChanged));

        public static void SetMinWidth(FrameworkElement obj, double value)
        {
            obj.SetValue(MinWidthProperty, value);
        }
        public static double GetMinWidth(FrameworkElement obj)
        {
            return (double)obj.GetValue(MinWidthProperty);
        }

        public static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MinWidth = GetProportionalDoubleProperty(e);
        }


        /// <summary>
        /// Independent MaxWidth property of FrameworkElement. Updates the original MaxWidth property
        /// </summary>
        public static DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMaxWidthPropertyChanged));

        public static void SetMaxWidth(FrameworkElement obj, double value)
        {
            obj.SetValue(MaxWidthProperty, value);
        }
        public static double GetMaxWidth(FrameworkElement obj)
        {
            return (double)obj.GetValue(MaxWidthProperty);
        }

        public static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MaxWidth = GetProportionalDoubleProperty(e);
        }

        /// <summary>
        /// Independent Height property of FrameworkElement. Updates the original Height property
        /// </summary>
        public static DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnHeightPropertyChanged));

        public static void SetHeight(FrameworkElement obj, double value)
        {
            obj.SetValue(HeightProperty, value);
        }
        public static double GetHeight(FrameworkElement obj)
        {
            return (double)obj.GetValue(HeightProperty);
        }

        public static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.Height = GetProportionalDoubleProperty(e);
        }

        /// <summary>
        /// Independent MinHeight property of FrameworkElement. Updates the original MinHeight property
        /// </summary>
        public static DependencyProperty MinHeightProperty = DependencyProperty.RegisterAttached("MinHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMinHeightPropertyChanged));

        public static void SetMinHeight(FrameworkElement obj, double value)
        {
            obj.SetValue(MinHeightProperty, value);
        }
        public static double GetMinHeight(FrameworkElement obj)
        {
            return (double)obj.GetValue(MinHeightProperty);
        }

        public static void OnMinHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MinHeight = GetProportionalDoubleProperty(e);
        }

        /// <summary>
        /// Independent MaxHeight property of FrameworkElement. Updates the original MaxHeight property
        /// </summary>
        public static DependencyProperty MaxHeightProperty = DependencyProperty.RegisterAttached("MaxHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMaxHeightPropertyChanged));

        public static void SetMaxHeight(FrameworkElement obj, double value)
        {
            obj.SetValue(MaxHeightProperty, value);
        }
        public static double GetMaxHeight(FrameworkElement obj)
        {
            return (double)obj.GetValue(MaxHeightProperty);
        }

        public static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MaxHeight = GetProportionalDoubleProperty(e);
        }

        /// <summary>
        /// Independent FontSize property of Control. Updates the original FontSize property
        /// </summary>
        public static DependencyProperty FontSizeProperty = DependencyProperty.RegisterAttached("FontSize",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(OnFontSizeChanged));

        public static void SetFontSize(FrameworkElement obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
        }
        public static double GetFontSize(FrameworkElement obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }

        public static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Control fe = d as Control;
            if (fe == null || e.NewValue == null)
                return;
            fe.FontSize = Math.Round(GetProportionalDoubleProperty(e));
        }


        public static double GetProportionalDoubleProperty(DependencyPropertyChangedEventArgs e)
        {
            double height;
            if (double.TryParse(e.NewValue.ToString(), out height))
            {
                if (height == double.NaN)
                    return double.NaN;
                else
                    return height / DpiUtils.GetToDeviceMatrix(null).M22;
            }
            return double.NaN;
        }

        // GridLength properties ********************************

        /// <summary>
        /// Independent Column Width property of ColumnDefinition. Updates the original Width property.
        /// </summary>
        public static DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnColumnWidthPropertyChanged));

        public static void SetColumnWidth(ColumnDefinition obj, double value)
        {
            obj.SetValue(ColumnWidthProperty, value);
        }
        public static double GetColumnWidth(FrameworkElement obj)
        {
            return (double)obj.GetValue(ColumnWidthProperty);
        }

        public static void OnColumnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnDefinition cd = d as ColumnDefinition;
            if (cd == null || e.NewValue == null)
                return;

            double width;
            if (double.TryParse(e.NewValue.ToString(), out width))
            {
                if (width == double.NaN)
                    cd.Width = new GridLength(1, GridUnitType.Star);
                else
                    cd.Width = new GridLength(width / DpiUtils.GetToDeviceMatrix(null).M11, GridUnitType.Pixel);
            }
        }

        /// <summary>
        /// Independent Row Width property of RowDefinition. Updates the original Height property.
        /// </summary>
        public static DependencyProperty RowHeightProperty = DependencyProperty.RegisterAttached("RowHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnRowHeightPropertyChanged));

        public static void SetRowHeight(RowDefinition obj, double value)
        {
            obj.SetValue(RowHeightProperty, value);
        }
        public static double GetRowHeight(RowDefinition obj)
        {
            return (double)obj.GetValue(RowHeightProperty);
        }

        public static void OnRowHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RowDefinition rd = d as RowDefinition;
            if (rd == null || e.NewValue == null)
                return;

            double height;
            if (double.TryParse(e.NewValue.ToString(), out height))
            {
                if (height == double.NaN)
                    rd.Height = new GridLength(1, GridUnitType.Star);
                else
                    rd.Height = new GridLength(height / DpiUtils.GetToDeviceMatrix(null).M22, GridUnitType.Pixel);
            }
        }


        // Thickness properties *********************************

        /// <summary>
        /// Independent Margin property of FrameworkElement. Updates the original Margin property
        /// </summary>
        public static DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin",
            typeof(Thickness), typeof(IndependentSize), new UIPropertyMetadata(OnMarginPropertyChanged));

        public static void SetMargin(FrameworkElement obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }
        public static Thickness GetMargin(FrameworkElement obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            try
            {
                Thickness source = (Thickness)e.NewValue;
                Thickness target = new Thickness(
                    source.Left / DpiUtils.GetToDeviceMatrix(null).M11,
                    source.Top / DpiUtils.GetToDeviceMatrix(null).M11,
                    source.Right / DpiUtils.GetToDeviceMatrix(null).M11,
                    source.Bottom / DpiUtils.GetToDeviceMatrix(null).M11);
                fe.Margin = target;
            }
            catch (Exception ex) { }
        }

    }
}

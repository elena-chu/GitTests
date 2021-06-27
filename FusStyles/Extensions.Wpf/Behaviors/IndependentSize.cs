using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    /// <summary>
    /// Class targeting the need to use the an absolute values like width, Height, Margin 
    /// that are independent from device unit size of Wpf(dependent on Display->TextSize).
    /// </summary>
    public class IndependentSize
    {
        static IndependentSize()
        {
            SystemEvents.DisplaySettingsChanged += new EventHandler(OnDisplaySettingsChanged);
        }

        private static void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            _screenProportionCalculated = false;
        }

        // Double properties ************************************

        /// <summary>
        /// Independent Width property of FrameworkElement. Updates the original Width property
        /// </summary>
        public static DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnWidthPropertyChanged));

        public static void SetWidth(FrameworkElement obj, double value) { obj.SetValue(WidthProperty, value); }
        public static double GetWidth(FrameworkElement obj) { return (double)obj.GetValue(WidthProperty); }

        public static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.Width = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent MinWidth property of FrameworkElement. Updates the original MinWidth property
        /// </summary>
        public static DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMinWidthPropertyChanged));

        public static void SetMinWidth(FrameworkElement obj, double value) { obj.SetValue(MinWidthProperty, value); }
        public static double GetMinWidth(FrameworkElement obj) { return (double)obj.GetValue(MinWidthProperty); }

        public static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MinWidth = GetProportionalDoublePropertyFromArgs(e);
        }


        /// <summary>
        /// Independent MaxWidth property of FrameworkElement. Updates the original MaxWidth property
        /// </summary>
        public static DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMaxWidthPropertyChanged));

        public static void SetMaxWidth(FrameworkElement obj, double value) { obj.SetValue(MaxWidthProperty, value); }
        public static double GetMaxWidth(FrameworkElement obj) { return (double)obj.GetValue(MaxWidthProperty); }

        public static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MaxWidth = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent Height property of FrameworkElement. Updates the original Height property
        /// </summary>
        public static DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnHeightPropertyChanged));

        public static void SetHeight(FrameworkElement obj, double value) { obj.SetValue(HeightProperty, value); }
        public static double GetHeight(FrameworkElement obj) { return (double)obj.GetValue(HeightProperty); }

        public static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.Height = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent MinHeight property of FrameworkElement. Updates the original MinHeight property
        /// </summary>
        public static DependencyProperty MinHeightProperty = DependencyProperty.RegisterAttached("MinHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMinHeightPropertyChanged));

        public static void SetMinHeight(FrameworkElement obj, double value) { obj.SetValue(MinHeightProperty, value); }
        public static double GetMinHeight(FrameworkElement obj) { return (double)obj.GetValue(MinHeightProperty); }

        public static void OnMinHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MinHeight = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent MaxHeight property of FrameworkElement. Updates the original MaxHeight property
        /// </summary>
        public static DependencyProperty MaxHeightProperty = DependencyProperty.RegisterAttached("MaxHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnMaxHeightPropertyChanged));

        public static void SetMaxHeight(FrameworkElement obj, double value) { obj.SetValue(MaxHeightProperty, value); }
        public static double GetMaxHeight(FrameworkElement obj) { return (double)obj.GetValue(MaxHeightProperty); }

        public static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            fe.MaxHeight = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent HorizontalOffset property of ContextMenu. Updates the original HorizontalOffset property
        /// </summary>
        public static DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnHorizontalOffsetPropertyChanged));

        public static void SetHorizontalOffset(FrameworkElement obj, double value) { obj.SetValue(HorizontalOffsetProperty, value); }
        public static double GetHorizontalOffset(FrameworkElement obj) { return (double)obj.GetValue(HorizontalOffsetProperty); }

        public static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            ContextMenu contextMenu = d as ContextMenu;
            Popup popup = d as Popup;

            if (contextMenu != null)
                contextMenu.HorizontalOffset = GetProportionalDoublePropertyFromArgs(e);

            if (popup != null)
                popup.HorizontalOffset = GetProportionalDoublePropertyFromArgs(e);
        }

        /// <summary>
        /// Independent VerticalOffset property of ContextMenu. Updates the original VerticalOffset property
        /// </summary>
        public static DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnVerticalOffsetPropertyChanged));

        public static void SetVerticalOffset(FrameworkElement obj, double value) { obj.SetValue(VerticalOffsetProperty, value); }
        public static double GetVerticalOffset(FrameworkElement obj) { return (double)obj.GetValue(VerticalOffsetProperty); }

        public static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            ContextMenu contextMenu = d as ContextMenu;
            Popup popup = d as Popup;

            if (contextMenu != null)
                contextMenu.VerticalOffset = GetProportionalDoublePropertyFromArgs(e);

            if (popup != null)
                popup.VerticalOffset = GetProportionalDoublePropertyFromArgs(e);
        }


        // FontSize *********************************************

        /// <summary>
        /// Independent FontSize property of Control. Updates the original FontSize property
        /// </summary>
        public static DependencyProperty FontSizeProperty = DependencyProperty.RegisterAttached("FontSize",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(OnFontSizeChanged));

        public static void SetFontSize(FrameworkElement obj, double value) { obj.SetValue(FontSizeProperty, value); }
        public static double GetFontSize(FrameworkElement obj) { return (double)obj.GetValue(FontSizeProperty); }

        public static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var fontSize = Math.Round(GetProportionalDoublePropertyFromArgs(e));
            if (fontSize <= 0)
                return;

            Control control = d as Control;
            TextBlock textBlock = d as TextBlock;
            if (control != null)
                control.FontSize = fontSize;
            else if (textBlock != null)
                textBlock.FontSize = fontSize;
        }

        public static double GetProportionalDoublePropertyFromArgs(DependencyPropertyChangedEventArgs e)
        {
            double size;
            if (double.TryParse(e.NewValue.ToString(), out size) && size != double.NaN)
                return CalculateProportionalDouble(size);
            return double.NaN;
        }

        private static bool _screenProportionCalculated = false;
        private static double _screenProportion;
        public static double CalculateProportionalDouble(double value)
        {
            if (!_screenProportionCalculated)
            {
                //Lena: Check Validity!!!
                _screenProportion = Math.Min(SystemParameters.PrimaryScreenHeight / 1080, SystemParameters.PrimaryScreenWidth /1920);
                _screenProportionCalculated = true;

                //var d = DpiUtils.GetToDeviceMatrix(null);
                //return height / DpiUtils.GetToDeviceMatrix(null).M22;

            }


            return value * _screenProportion;
        }

        // GridLength properties ********************************

        /// <summary>
        /// Independent Column Width property of ColumnDefinition. Updates the original Width property.
        /// </summary>
        public static DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnColumnWidthPropertyChanged));

        public static void SetColumnWidth(ColumnDefinition obj, double value) { obj.SetValue(ColumnWidthProperty, value); }
        public static double GetColumnWidth(FrameworkElement obj) { return (double)obj.GetValue(ColumnWidthProperty); }

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
                    cd.Width = new GridLength(GetProportionalDoublePropertyFromArgs(e), GridUnitType.Pixel);
            }
        }

        /// <summary>
        /// Independent Row Width property of RowDefinition. Updates the original Height property.
        /// </summary>
        public static DependencyProperty RowHeightProperty = DependencyProperty.RegisterAttached("RowHeight",
            typeof(double), typeof(IndependentSize), new UIPropertyMetadata(double.NaN, OnRowHeightPropertyChanged));

        public static void SetRowHeight(RowDefinition obj, double value) { obj.SetValue(RowHeightProperty, value); }
        public static double GetRowHeight(RowDefinition obj) { return (double)obj.GetValue(RowHeightProperty); }

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
                    rd.Height = new GridLength(GetProportionalDoublePropertyFromArgs(e), GridUnitType.Pixel);
            }
        }

        // Thickness properties *********************************

        /// <summary>
        /// Independent Margin property of FrameworkElement. Updates the original Margin property
        /// </summary>
        public static DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin",
            typeof(Thickness), typeof(IndependentSize), new UIPropertyMetadata(OnMarginPropertyChanged));

        public static void SetMargin(FrameworkElement obj, Thickness value) { obj.SetValue(MarginProperty, value); }
        public static Thickness GetMargin(FrameworkElement obj) { return (Thickness)obj.GetValue(MarginProperty); }

        public static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe == null || e.NewValue == null)
                return;
            Thickness source = (Thickness)e.NewValue;
            Thickness target = new Thickness(
                CalculateProportionalDouble(source.Left),
                CalculateProportionalDouble(source.Top),
                CalculateProportionalDouble(source.Right),
                CalculateProportionalDouble(source.Bottom));
            fe.Margin = target;
        }


        /// <summary>
        /// Independent Padding property of FrameworkElement. Updates the original Padding property
        /// </summary>
        public static DependencyProperty PaddingProperty = DependencyProperty.RegisterAttached("Padding",
            typeof(Thickness), typeof(IndependentSize), new UIPropertyMetadata(OnPaddingPropertyChanged));

        public static void SetPadding(FrameworkElement obj, Thickness value) { obj.SetValue(PaddingProperty, value); }
        public static Thickness GetPadding(FrameworkElement obj) { return (Thickness)obj.GetValue(PaddingProperty); }

        public static void OnPaddingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            Border border = d as Border;
            TextBlock textBlock = d as TextBlock;
            Control control = d as Control;

            if (border != null || textBlock != null || control != null)
            {
                Thickness source = (Thickness)e.NewValue;
                Thickness target = new Thickness(
                    CalculateProportionalDouble(source.Left),
                    CalculateProportionalDouble(source.Top),
                    CalculateProportionalDouble(source.Right),
                    CalculateProportionalDouble(source.Bottom));

                if (border != null)
                    border.Padding = target;
                if (textBlock != null)
                    textBlock.Padding = target;
                if (control != null)
                    control.Padding = target;
            }
        }

        /// <summary>
        /// Independent CornerRadius property of FrameworkElement. Updates the original CornerRadius property
        /// </summary>
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(IndependentSize), new UIPropertyMetadata(OnCornerRadiusPropertyChanged));
        public static void SetCornerRadius(FrameworkElement obj, CornerRadius value) { obj.SetValue(CornerRadiusProperty, value); }
        public static CornerRadius GetCornerRadius(FrameworkElement obj) { return (CornerRadius)obj.GetValue(CornerRadiusProperty); }

        public static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            Border border = d as Border;
            if (border != null)
            {
                CornerRadius source = (CornerRadius)e.NewValue;
                border.CornerRadius = new CornerRadius(
                    CalculateProportionalDouble(source.TopLeft),
                    CalculateProportionalDouble(source.TopRight),
                    CalculateProportionalDouble(source.BottomRight),
                    CalculateProportionalDouble(source.BottomLeft));
            }
        }
    }
}

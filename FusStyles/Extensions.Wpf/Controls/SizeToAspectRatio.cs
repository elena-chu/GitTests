using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ws.Extensions.UI.Wpf.Controls
{
    /// <summary>
    /// SizeToAspectRatio - Class helping to keep a Grid with proportions allowing the all inner grids to be a squares.
    /// Proportion depends on how many rows and columns contains the grid and its size depends on the size of parent element.
    /// Also places the grid inside parent element.
    /// </summary>
    public class SizeToAspectRatio : Decorator
    {
        bool _isOneTimeArranged = false;

        #region Dependency Properties

        /// <summary>
        /// Number of columns in grid holding squares
        /// </summary>
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns",
            typeof(int), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Number of rows in grid holding squares
        /// </summary>
        public static DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
                typeof(int), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// IsHeightBased Property defines how is calculated grid's size.
        /// True - the resulting grid size is depending only on parent element's Height. Position of grid is Right aligned
        /// False - the resulting grid size is fully depending on parent element's size and placed centered inside parent element
        /// </summary>
        public static DependencyProperty IsHeightBasedProperty = DependencyProperty.Register("IsHeightBased",
            typeof(bool), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool IsHeightBased
        {
            get { return (bool)GetValue(IsHeightBasedProperty); }
            set { SetValue(IsHeightBasedProperty, value); }
        }

        /// <summary>
        /// Specified whether action of sizing should be done only ones - generally when first Framework Element is created and loaded
        /// and already has reasonable size.It happens slightly after creation.
        /// Further changes inside or outside of element don't change its size.
        /// </summary>
        public static DependencyProperty IsOneTimeProperty = DependencyProperty.Register("IsOneTime",
            typeof(bool), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool IsOneTime
        {
            get { return (bool)GetValue(IsOneTimeProperty); }
            set { SetValue(IsOneTimeProperty, value); }
        }

        #endregion

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var calculatedWidth = arrangeSize.Height * Columns / Rows;
            var calculatedHeight = arrangeSize.Width * Rows / Columns;

            if (double.IsNaN(calculatedWidth) || double.IsNaN(calculatedHeight))
            {
                Child.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
                return arrangeSize;
            }

            if (IsHeightBased)
            {
                if (this.MaxHeight != double.NaN && arrangeSize.Height > this.MaxHeight)
                {
                    calculatedWidth = this.MaxHeight * Columns / Rows;
                    Child.Arrange(new Rect(0, 0, calculatedWidth, this.MaxHeight));
                }
                else
                {
                    Child.Arrange(new Rect(0, 0, calculatedWidth, arrangeSize.Height));
                }
                arrangeSize.Width = calculatedWidth;
                this.Width = calculatedWidth;
            }
            else
            {
                if (calculatedWidth <= arrangeSize.Width)
                {
                    Child.Arrange(new Rect((arrangeSize.Width - calculatedWidth) / 2, 0, calculatedWidth, arrangeSize.Height));
                    calculatedHeight = arrangeSize.Height;
                }
                else
                {
                    Child.Arrange(new Rect(0, (arrangeSize.Height - calculatedHeight) / 2, arrangeSize.Width, calculatedHeight));
                    calculatedWidth = arrangeSize.Width;
                }

                bool isResonableSize = arrangeSize.Width > 100 && arrangeSize.Height > 100;
                if (IsOneTime && Child is FrameworkElement && !_isOneTimeArranged && isResonableSize)
                {
                    ((FrameworkElement)Child).Width = calculatedWidth;
                    ((FrameworkElement)Child).Height = calculatedHeight;
                    _isOneTimeArranged = true;
                }
            }

            return arrangeSize;
        }
    }
}

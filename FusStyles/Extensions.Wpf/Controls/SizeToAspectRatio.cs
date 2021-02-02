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
        /// <summary>
        /// Number of row in grid holding squares
        /// </summary>
		public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns",
			typeof(int), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Number of columns in grid holding squares
        /// </summary>
		public static DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
			typeof(int), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		public int Columns
		{
			get { return (int)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

        /// <summary>
        /// HeightBased Property defines how is calculated grid's size.
        /// True - the resulting grid size is depending only on parent element's Height. Position of grid is Right aligned
        /// False - the resulting grid size is fully depending on parent element's size and placed centered inside parent element
        /// </summary>
        public static DependencyProperty HeightBasedProperty = DependencyProperty.Register("HeightBased",
            typeof(bool), typeof(SizeToAspectRatio), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool HeightBased
        {
            get { return (bool)GetValue(HeightBasedProperty); }
            set { SetValue(HeightBasedProperty, value); }
        }

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

            if(HeightBased)
            {
                Child.Arrange(new Rect(0, 0, calculatedWidth, arrangeSize.Height));
                arrangeSize.Width = calculatedWidth;
                this.Width = calculatedWidth;
            }
            else
            {
                if (calculatedWidth <= arrangeSize.Width)
                {
                    Child.Arrange(new Rect((arrangeSize.Width - calculatedWidth) / 2, 0, calculatedWidth, arrangeSize.Height));
                }
                else
                {
                    Child.Arrange(new Rect(0, (arrangeSize.Height - calculatedHeight) / 2, arrangeSize.Width, calculatedHeight));
                }
            }

            return arrangeSize;
		}
	}
}

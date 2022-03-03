using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InsightecGrid.ResizableGrid
{
    public partial class ResizableGridView : UserControl
    {
        #region Start, End

        public ResizableGridView()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        #endregion


        #region Grid External Properties

        /// <summary>
        /// Max Range from Origin, in mm
        /// </summary>
        public double MaxRangeFromOrigin
        {
            get { return (double)GetValue(MaxRangeFromOriginProperty); }
            set { SetValue(MaxRangeFromOriginProperty, value); }
        }
        public static readonly DependencyProperty MaxRangeFromOriginProperty =
            DependencyProperty.Register(nameof(MaxRangeFromOrigin), typeof(double), typeof(ResizableGridView), new PropertyMetadata(80.0, OnGridExternalPropertiesChanged));

        /// <summary>
        /// Resolution in Pixels per mm
        /// </summary>
        public double Resolution
        {
            get { return (double)GetValue(ResolutionProperty); }
            set { SetValue(ResolutionProperty, value); }
        }
        public static readonly DependencyProperty ResolutionProperty =
            DependencyProperty.Register(nameof(Resolution), typeof(double), typeof(ResizableGridView), new PropertyMetadata(4.0, OnGridExternalPropertiesChanged));

        private static void OnGridExternalPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ResizableGridView resizableGridView)
                resizableGridView.UpdateGrid();
        }

        private void UpdateGrid()
        {
            ResizeTile();
            LimitGridLineQuarters();
            LimitAxes();
            SetTicks();
            AlignToOrigin();
        }

        #endregion


        #region Tiles

        private double ResolutionToTickFrequency()
        {
            if (Resolution <= 4)
                return 20;
            else if (Resolution <= 8)
                return 10;
            else if (Resolution <= 16)
                return 5;
            else if (Resolution <= 32)
                return 4;
            else if (Resolution <= 64)
                return 2;
            else
                return 1;
        }

        public double TileSize
        {
            get { return (double)GetValue(TileSizeProperty); }
            private set { SetValue(TileSizeProperty, value); }
        }
        public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(nameof(TileSize), typeof(double), typeof(ResizableGridView), new PropertyMetadata(1.0));

        public Rect GridViewport
        {
            get { return (Rect)GetValue(GridViewportProperty); }
            private set { SetValue(GridViewportProperty, value); }
        }
        public static readonly DependencyProperty GridViewportProperty = DependencyProperty.Register(nameof(GridViewport), typeof(Rect), typeof(ResizableGridView), new PropertyMetadata(new Rect(0,0,30,30)));

        private void ResizeTile()
        {
            double tileSize = Math.Round(Resolution * ResolutionToTickFrequency());
            TileSize = tileSize;
            if (PlaygroundCanvas.ActualWidth > 0 && Resolution > 0)
                GridViewport = new Rect(0, 0, tileSize, tileSize);
        }
        
        #endregion


        #region Grid Line Quarters

        private void LimitGridLineQuarters()
        {
            double limitation = MaxRangeFromOrigin * Resolution;
            SouthEastGridLinesRectangle.MaxWidth = limitation;
            SouthEastGridLinesRectangle.MaxHeight = limitation;
            SouthWestGridLinesRectangle.MaxWidth = limitation;
            SouthWestGridLinesRectangle.MaxHeight = limitation;
            NorthEastGridLinesRectangle.MaxWidth = limitation;
            NorthEastGridLinesRectangle.MaxHeight = limitation;
            NorthWestGridLinesRectangle.MaxWidth = limitation;
            NorthWestGridLinesRectangle.MaxHeight = limitation;
        }

        private void AlignGridLinesToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            Canvas.SetLeft(SouthEastGridLinesRectangle, Origin.X);
            Canvas.SetTop(SouthEastGridLinesRectangle, Origin.Y);
            SouthEastGridLinesRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);
            SouthEastGridLinesRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetRight(SouthWestGridLinesRectangle, PlaygroundCanvas.ActualWidth - Origin.X);
            Canvas.SetTop(SouthWestGridLinesRectangle, Origin.Y);
            SouthWestGridLinesRectangle.Width = Math.Max(Origin.X,0);
            SouthWestGridLinesRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetRight(NorthWestGridLinesRectangle, PlaygroundCanvas.ActualWidth - Origin.X);
            Canvas.SetBottom(NorthWestGridLinesRectangle, PlaygroundCanvas.ActualHeight - Origin.Y);
            NorthWestGridLinesRectangle.Width = Math.Max(Origin.X, 0);
            NorthWestGridLinesRectangle.Height = Math.Max(Origin.Y, 0);

            Canvas.SetLeft(NorthEastGridLinesRectangle, Origin.X);
            Canvas.SetBottom(NorthEastGridLinesRectangle, PlaygroundCanvas.ActualHeight - Origin.Y);
            NorthEastGridLinesRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);
            NorthEastGridLinesRectangle.Height = Math.Max(Origin.Y, 0);
        }

        #endregion


        #region Axes

        private void LimitAxes()
        {
            double limitation = MaxRangeFromOrigin * Resolution;
            EastAxisRectangle.MaxWidth = limitation;
            WestAxisRectangle.MaxWidth = limitation;
            SouthAxisRectangle.MaxHeight = limitation;
            NorthAxisRectangle.MaxHeight = limitation;
        }

        private void AlignAxesToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            double halfTile = Math.Round(TileSize / 2);

            Canvas.SetLeft(EastAxisRectangle, Origin.X);
            Canvas.SetTop(EastAxisRectangle, Origin.Y - halfTile);
            EastAxisRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);

            Canvas.SetRight(WestAxisRectangle, PlaygroundCanvas.ActualWidth - Origin.X);
            Canvas.SetTop(WestAxisRectangle, Origin.Y - halfTile);
            WestAxisRectangle.Width = Math.Max(Origin.X, 0);

            Canvas.SetLeft(SouthAxisRectangle, Origin.X - halfTile);
            Canvas.SetTop(SouthAxisRectangle, Origin.Y);
            SouthAxisRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetLeft(NorthAxisRectangle, Origin.X - halfTile);
            Canvas.SetBottom(NorthAxisRectangle, PlaygroundCanvas.ActualHeight - Origin.Y);
            NorthAxisRectangle.Height = Math.Max(Origin.Y, 0);
        }

        #endregion


        #region Tick Labels

        public ObservableCollection<double> TickLabels
        {
            get { return (ObservableCollection<double>)GetValue(TickLabelsProperty); }
            private set { SetValue(TickLabelsProperty, value); }
        }
        public static readonly DependencyProperty TickLabelsProperty = DependencyProperty.Register(nameof(TickLabels), typeof(ObservableCollection<double>), typeof(ResizableGridView));

        private void SetTicks()
        {
            SetTickLabels();
            ResizeTickLabelsLengthToTiles();
        }

        private void SetTickLabels()
        {
            double tickFrequency = ResolutionToTickFrequency();
            int numberOfTicks = Math.Min((int)(MaxRangeFromOrigin / tickFrequency), 20);
            ObservableCollection<double> tickLabels = new ObservableCollection<double>();
            for (int i = 1; i <= numberOfTicks; i++)
                tickLabels.Add(i * tickFrequency);
            TickLabels = tickLabels;
        }

        private void ResizeTickLabelsLengthToTiles()
        {
            if (TickLabels == null || !TickLabels.Any())
                return;

            double tickLabelsLength = TileSize * TickLabels.Count;

            EastTopTickLabelsItemsControl.Width = tickLabelsLength;
            EastBottomTickLabelsItemsControl.Width = tickLabelsLength;
            WestTopTickLabelsItemsControl.Width = tickLabelsLength;
            WestBottomTickLabelsItemsControl.Width = tickLabelsLength;
            SouthLeftTickLabelsItemsControl.Height = tickLabelsLength;
            SouthRightTickLabelsItemsControl.Height = tickLabelsLength;
            NorthLeftTickLabelsItemsControl.Height = tickLabelsLength;
            NorthRightTickLabelsItemsControl.Height = tickLabelsLength;
        }

        private void AlignLabelsToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            double halfTile = Math.Round(TileSize / 2);

            Canvas.SetLeft(EastTopTickLabelsItemsControl, Origin.X + halfTile);
            Canvas.SetTop(EastTopTickLabelsItemsControl, Origin.Y - TileSize);

            Canvas.SetLeft(EastBottomTickLabelsItemsControl, Origin.X + halfTile);
            Canvas.SetTop(EastBottomTickLabelsItemsControl, Origin.Y);

            Canvas.SetRight(WestTopTickLabelsItemsControl, PlaygroundCanvas.ActualWidth - Origin.X + halfTile);
            Canvas.SetTop(WestTopTickLabelsItemsControl, Origin.Y - TileSize);

            Canvas.SetRight(WestBottomTickLabelsItemsControl, PlaygroundCanvas.ActualWidth - Origin.X + halfTile);
            Canvas.SetTop(WestBottomTickLabelsItemsControl, Origin.Y);

            Canvas.SetLeft(SouthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetTop(SouthLeftTickLabelsItemsControl, Origin.Y + halfTile);

            Canvas.SetLeft(SouthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetTop(SouthRightTickLabelsItemsControl, Origin.Y + halfTile);

            Canvas.SetLeft(NorthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetBottom(NorthLeftTickLabelsItemsControl, PlaygroundCanvas.ActualHeight - Origin.Y + halfTile);

            Canvas.SetLeft(NorthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetBottom(NorthRightTickLabelsItemsControl, PlaygroundCanvas.ActualHeight - Origin.Y + halfTile);
        }

        #endregion


        #region Origin

        public Point Origin
        {
            get { return (Point)GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }
        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(nameof(Origin), typeof(Point), typeof(ResizableGridView), new PropertyMetadata(new Point(0, 0), OnOriginChanged));

        private static void OnOriginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ResizableGridView resizableGridView)
                resizableGridView.AlignToOrigin();
        }

        private void AlignToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            AlignGridLinesToOrigin();
            AlignAxesToOrigin();
            AlignLabelsToOrigin();

            Canvas.SetLeft(OriginEllipse, Origin.X - OriginEllipse.ActualWidth / 2);
            Canvas.SetTop(OriginEllipse, Origin.Y - OriginEllipse.ActualHeight / 2);
        }

        #endregion


        #region Playground

        private void PlaygroundCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateGrid();
        }
        
        #endregion
    }
}

using System;
using System.Collections.ObjectModel;
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
            ResizeTiles();
            AlignToOrigin();
        }

        #endregion


        #region Tiles

        public double TileSize
        {
            get { return (double)GetValue(TileSizeProperty); }
            set { SetValue(TileSizeProperty, value); }
        }
        public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(nameof(TileSize), typeof(double), typeof(ResizableGridView), new PropertyMetadata(1.0));

        public Rect GridViewport
        {
            get { return (Rect)GetValue(GridViewportProperty); }
            set { SetValue(GridViewportProperty, value); }
        }
        public static readonly DependencyProperty GridViewportProperty = DependencyProperty.Register(nameof(GridViewport), typeof(Rect), typeof(ResizableGridView), new PropertyMetadata(new Rect(0,0,30,30)));

        private void ResizeTiles()
        {
            double tileSize = Math.Round(Resolution * 10); // LA TODO: calc according to jumps
            TileSize = tileSize;
            if (PlaygroundCanvas.ActualWidth > 0 && Resolution > 0)
                GridViewport = new Rect(0, 0, tileSize, tileSize);
            ResizeTickLabelsLengthToTiles();
        }

        private void AlignTilesToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            Canvas.SetLeft(SouthEastGridLinesRectangle, Origin.X);
            Canvas.SetTop(SouthEastGridLinesRectangle, Origin.Y);
            SouthEastGridLinesRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);
            SouthEastGridLinesRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetLeft(SouthWestGridLinesRectangle, 0);
            Canvas.SetTop(SouthWestGridLinesRectangle, Origin.Y);
            SouthWestGridLinesRectangle.Width = Math.Max(Origin.X,0);
            SouthWestGridLinesRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetLeft(NorthWestGridLinesRectangle, 0);
            Canvas.SetTop(NorthWestGridLinesRectangle, 0);
            NorthWestGridLinesRectangle.Width = Math.Max(Origin.X, 0);
            NorthWestGridLinesRectangle.Height = Math.Max(Origin.Y, 0);

            Canvas.SetLeft(NorthEastGridLinesRectangle, Origin.X);
            Canvas.SetTop(NorthEastGridLinesRectangle, 0);
            NorthEastGridLinesRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);
            NorthEastGridLinesRectangle.Height = Math.Max(Origin.Y, 0);
        }

        #endregion


        #region Axes

        private void AlignAxesToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            double halfTile = Math.Round(TileSize / 2);

            Canvas.SetLeft(EastAxisRectangle, Origin.X);
            Canvas.SetTop(EastAxisRectangle, Origin.Y - halfTile);
            EastAxisRectangle.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X, 0);

            Canvas.SetLeft(WestAxisRectangle, 0);
            Canvas.SetTop(WestAxisRectangle, Origin.Y - halfTile);
            WestAxisRectangle.Width = Math.Max(Origin.X, 0);

            Canvas.SetLeft(SouthAxisRectangle, Origin.X - halfTile);
            Canvas.SetTop(SouthAxisRectangle, Origin.Y);
            SouthAxisRectangle.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y, 0);

            Canvas.SetLeft(NorthAxisRectangle, Origin.X - halfTile);
            Canvas.SetTop(NorthAxisRectangle, 0);
            NorthAxisRectangle.Height = Math.Max(Origin.Y, 0);
        }

        #endregion


        #region Tick Labels

        public ObservableCollection<double> TickLabels { get; set; } = new ObservableCollection<double>()
        {
            10, 20, 30, 40, 50, 60, 70, 80
        };

        private void ResizeTickLabelsLengthToTiles()
        {
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

            Canvas.SetLeft(WestTopTickLabelsItemsControl, Origin.X - WestTopTickLabelsItemsControl.ActualWidth - halfTile);
            Canvas.SetTop(WestTopTickLabelsItemsControl, Origin.Y - TileSize);

            Canvas.SetLeft(WestBottomTickLabelsItemsControl, Origin.X - WestBottomTickLabelsItemsControl.ActualWidth - halfTile);
            Canvas.SetTop(WestBottomTickLabelsItemsControl, Origin.Y);

            Canvas.SetLeft(SouthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetTop(SouthLeftTickLabelsItemsControl, Origin.Y + halfTile);

            Canvas.SetLeft(SouthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetTop(SouthRightTickLabelsItemsControl, Origin.Y + halfTile);

            Canvas.SetLeft(NorthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetTop(NorthLeftTickLabelsItemsControl, Origin.Y - NorthLeftTickLabelsItemsControl.ActualHeight - halfTile);

            Canvas.SetLeft(NorthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetTop(NorthRightTickLabelsItemsControl, Origin.Y - NorthRightTickLabelsItemsControl.ActualHeight - halfTile);
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

            AlignTilesToOrigin();
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

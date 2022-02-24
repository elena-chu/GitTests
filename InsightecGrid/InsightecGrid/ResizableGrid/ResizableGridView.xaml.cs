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

        private void UpdateGrid()
        {
            ResizeTiles();
            AlignToOrigin();
        }

        #endregion


        #region Tiles

        /// <summary>
        /// Resolution in Pixels per mm
        /// </summary>
        public double Resolution
        {
            get { return (double)GetValue(ResolutionProperty); }
            set { SetValue(ResolutionProperty, value); }
        }
        public static readonly DependencyProperty ResolutionProperty =
            DependencyProperty.Register(nameof(Resolution), typeof(double), typeof(ResizableGridView), new PropertyMetadata(30.0, OnResolutionChanged));

        private static void OnResolutionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ResizableGridView resizableGridView)
                resizableGridView.UpdateGrid();
        }

        public double TileSize
        {
            get { return (double)GetValue(TileSizeProperty); }
            set { SetValue(TileSizeProperty, value); }
        }
        public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(nameof(TileSize), typeof(double), typeof(ResizableGridView), new PropertyMetadata(30.0));

        public Rect GridViewport
        {
            get { return (Rect)GetValue(GridViewportProperty); }
            set { SetValue(GridViewportProperty, value); }
        }
        public static readonly DependencyProperty GridViewportProperty = DependencyProperty.Register(nameof(GridViewport), typeof(Rect), typeof(ResizableGridView), new PropertyMetadata(new Rect(0,0,30,30)));

        private void ResizeTiles()
        {
            double tileSize = Math.Round(Resolution);
            TileSize = tileSize;
            if (PlaygroundCanvas.ActualWidth > 0 && Resolution > 0)
                GridViewport = new Rect(0, 0, tileSize, tileSize);
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

        public ObservableCollection<double> TickLabels { get; set; } = new ObservableCollection<double>()
        {
            10, 20, 30, 40, 50, 60, 70, 80
        };

        private void AlignLabelsToOrigin()
        {
            if (PlaygroundCanvas.ActualWidth <= 0 || PlaygroundCanvas.ActualHeight <= 0)
                return;

            double halfTile = Math.Round(TileSize / 2);

            Canvas.SetLeft(EastTopTickLabelsItemsControl, Origin.X + halfTile);
            Canvas.SetTop(EastTopTickLabelsItemsControl, Origin.Y - TileSize);
            EastTopTickLabelsItemsControl.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X - halfTile, 0);

            Canvas.SetLeft(EastBottomTickLabelsItemsControl, Origin.X + halfTile);
            Canvas.SetTop(EastBottomTickLabelsItemsControl, Origin.Y);
            EastBottomTickLabelsItemsControl.Width = Math.Max(PlaygroundCanvas.ActualWidth - Origin.X - halfTile, 0);

            Canvas.SetLeft(WestTopTickLabelsItemsControl, 0);
            Canvas.SetTop(WestTopTickLabelsItemsControl, Origin.Y - TileSize);
            WestTopTickLabelsItemsControl.Width = Math.Max(Origin.X - halfTile, 0);

            Canvas.SetLeft(WestBottomTickLabelsItemsControl, 0);
            Canvas.SetTop(WestBottomTickLabelsItemsControl, Origin.Y);
            WestBottomTickLabelsItemsControl.Width = Math.Max(Origin.X - halfTile, 0);

            Canvas.SetLeft(SouthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetTop(SouthLeftTickLabelsItemsControl, Origin.Y + halfTile);
            SouthLeftTickLabelsItemsControl.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y - halfTile, 0);

            Canvas.SetLeft(SouthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetTop(SouthRightTickLabelsItemsControl, Origin.Y + halfTile);
            SouthRightTickLabelsItemsControl.Height = Math.Max(PlaygroundCanvas.ActualHeight - Origin.Y - halfTile, 0);

            Canvas.SetLeft(NorthLeftTickLabelsItemsControl, Origin.X - TileSize);
            Canvas.SetTop(NorthLeftTickLabelsItemsControl, 0);
            NorthLeftTickLabelsItemsControl.Height = Math.Max(Origin.Y - halfTile, 0);

            Canvas.SetLeft(NorthRightTickLabelsItemsControl, Origin.X);
            Canvas.SetTop(NorthRightTickLabelsItemsControl, 0);
            NorthRightTickLabelsItemsControl.Height = Math.Max(Origin.Y - halfTile, 0);
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

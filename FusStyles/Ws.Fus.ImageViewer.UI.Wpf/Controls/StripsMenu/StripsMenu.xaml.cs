using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    /// <summary>
    /// Interaction logic for StripsMenu.xaml
    /// </summary>
    public partial class StripsMenu : UserControl
    {
        public StripsMenu()
        {
            InitializeComponent();
        }

        //private static readonly ILogger _logger = Log.ForContext<StripsMenu>();


        #region Drag and Drop

        //private DragNDropHelper<IStripVm<IStrip>> _stripsDragNDrop;

        public string DragNDrop { get; set; } = "StripDnD";

        #endregion


        #region Groups

        /// <summary>
        /// To prevent multiple groups initialization evenry time when <see cref="FrameworkElement.Loaded"/> fires
        /// </summary>
        private bool _groupsInitialized;

        private void InitializeStripGroups()
        {
            if (_groupsInitialized)
                return;

            if (lvStrips == null || lvStrips.ItemsSource == null)
                return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvStrips.ItemsSource);

            if (!string.IsNullOrWhiteSpace(Group1))
            {
                var gd = Group1Converter == null ? new PropertyGroupDescription(Group1) : new PropertyGroupDescription(Group1, Group1Converter);
                view.GroupDescriptions.Add(gd);
            }

            if (!string.IsNullOrWhiteSpace(Group2))
            {
                var gd = Group2Converter == null ? new PropertyGroupDescription(Group2) : new PropertyGroupDescription(Group2, Group2Converter);
                view.GroupDescriptions.Add(gd);
            }

            _groupsInitialized = true;
        }

        private void InitGroupsTemp()
        {
            if (_groupsInitialized || lvStrips == null || lvStrips.ItemsSource == null)
                return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvStrips.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription("OrientationString"));
            _groupsInitialized = true;
        }

        /// <summary>
        /// Default Group1: <see cref="IStripVm{T}"/> with <see cref="StripToCategoryConverter"/>
        /// </summary>
        public string Group1
        {
            get { return (string)GetValue(Group1Property); }
            set { SetValue(Group1Property, value); }
        }
        public static readonly DependencyProperty Group1Property = DependencyProperty.Register(nameof(Group1), typeof(string), typeof(StripsMenu), new PropertyMetadata("."));

        public IValueConverter Group1Converter
        {
            get { return (IValueConverter)GetValue(Group1ConverterProperty); }
            set { SetValue(Group1ConverterProperty, value); }
        }
        public static readonly DependencyProperty Group1ConverterProperty = DependencyProperty.Register(nameof(Group1Converter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToCategoryConverter()));

        /// <summary>
        /// Default Group1: <see cref="IStripVm{T}"/> with <see cref="StripToStudyNumberConverter"/>
        /// </summary>
        public string Group2
        {
            get { return (string)GetValue(Group2Property); }
            set { SetValue(Group2Property, value); }
        }
        public static readonly DependencyProperty Group2Property = DependencyProperty.Register(nameof(Group2), typeof(string), typeof(StripsMenu), new PropertyMetadata("."));


        public IValueConverter Group2Converter
        {
            get { return (IValueConverter)GetValue(Group2ConverterProperty); }
            set { SetValue(Group2ConverterProperty, value); }
        }
        public static readonly DependencyProperty Group2ConverterProperty =
            DependencyProperty.Register(nameof(Group2Converter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToStudyNumberConverter()));

        #endregion


        #region Image

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
        public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register(nameof(ImageSize), typeof(double), typeof(StripsMenu), new PropertyMetadata(100.0));

        //public IValueConverter ImageConverter
        //{
        //    get { return (IValueConverter)GetValue(ImageConverterProperty); }
        //    set { SetValue(ImageConverterProperty, value); }
        //}
        //public static readonly DependencyProperty ImageConverterProperty = DependencyProperty.Register(nameof(ImageConverter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToImageConverter()));

        #endregion


        #region Strips

        public IEnumerable<IStrip> Strips
        {
            get { return (IEnumerable<IStrip>)GetValue(StripsProperty); }
            set { SetValue(StripsProperty, value); }
        }
        public static readonly DependencyProperty StripsProperty = DependencyProperty.Register(nameof(Strips), typeof(IEnumerable<IStrip>), typeof(StripsMenu));


        //public IStripActionsHolder StripActionsHolder
        //{
        //    get { return (IStripActionsHolder)GetValue(StripActionsHolderProperty); }
        //    set { SetValue(StripActionsHolderProperty, value); }
        //}

        private void StripsMenu_Loaded(object sender, RoutedEventArgs e)
        {
            //InitializeStripGroups();
            InitGroupsTemp();
            //if (DataContext is IStripActionsHolder && StripActionsHolder == null)
            //    StripActionsHolder = DataContext as IStripActionsHolder;
        }

        private void Strips_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //_stripsDragNDrop = new DragNDropHelper<IStripVm<IStrip>>(sender, e, DragNDrop, CreateStripDnDData);
        }

        private void Strips_PreviewMouseMove(object sender, MouseEventArgs e) { }/*=> _stripsDragNDrop?.PreviewMouseMove(sender, e);*/

        //private DragNDropHelper<IStripVm<IStrip>>.DragNDropData CreateStripDnDData(object sender, MouseEventArgs e)
        //{
        //    // Get the dragged ListViewItem
        //    ListView listView = sender as ListView;

        //    //var strip = (IStrip)listView.SelectedItem;
        //    //if (strip == null)
        //    //    return null;

        //    var stripVm = listView.SelectedItem as IStripVm<IStrip>;
        //    if (stripVm == null)
        //    {
        //        _logger.Warning("Item of unexpected type got upon DragNDrop: {@item}", listView.SelectedItem);
        //        return null;
        //    }

        //    var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(listView.SelectedItem);

        //    return new DragNDropHelper<IStripVm<IStrip>>.DragNDropData
        //    {
        //        Data = stripVm,
        //        DragSource = listViewItem,
        //        Effects = DragDropEffects.Copy
        //    };
        //}

        //private void OnItemPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    lvStrips.Tag = null;
        //    ListViewItem lvi = sender as ListViewItem;
        //    if(lvi != null && lvi.Content is IStrip) 
        //        lvStrips.Tag = lvi.Content as IStrip;
        //}

        //private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    if(StripActionsHolder == null)
        //    {
        //        e.Handled = true;
        //        return;
        //    }

        //    ListViewItem li = (e.OriginalSource as DependencyObject).ParentOfType<ListViewItem>();
        //    if (li == null)
        //    {
        //        e.Handled = true;
        //        lvStrips.Tag = null;
        //    }
        //    else
        //    {
        //        lvStrips.Tag = li.Content;
        //        StripActionsHolder.RaiseInvalidateStripCommands();
        //    }
        //}

        #endregion
    }
}

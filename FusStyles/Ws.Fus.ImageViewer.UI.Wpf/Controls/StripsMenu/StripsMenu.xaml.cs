//using Ws.Extensions.UI.Wpf.Patterns;
//using Ws.Fus.Strips.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;
//using Serilog;
using Ws.Extensions.UI.Wpf.Utils;
using System.Collections;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    /// <summary>
    /// Interaction logic for StripsMenu.xaml
    /// </summary>
    public partial class StripsMenu : UserControl
    {
        //private static readonly ILogger _logger = Log.ForContext<StripsMenu>();

        //private DragNDropHelper<IStripVm<IStrip>> _stripsDragNDrop;
        /// <summary>
        /// To prevent multiple groups initialization evenry time when <see cref="FrameworkElement.Loaded"/> fires
        /// </summary>
        private bool _groupsInitialized;

        public static readonly DependencyProperty StripTemplateSelectorProperty =
            DependencyProperty.Register(nameof(StripTemplateSelector), typeof(DataTemplateSelector), typeof(StripsMenu), new PropertyMetadata(new StripsMenuDefaultDtSelector()));

        /// <summary>
        /// Default Group1: <see cref="IStripVm{T}"/> with <see cref="StripToCategoryConverter"/>
        /// </summary>
        public static readonly DependencyProperty Group1Property =
            DependencyProperty.Register(nameof(Group1), typeof(string), typeof(StripsMenu), new PropertyMetadata("."));

        public static readonly DependencyProperty Group1ConverterProperty =
            DependencyProperty.Register(nameof(Group1Converter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToCategoryConverter()));

        /// <summary>
        /// Default Group1: <see cref="IStripVm{T}"/> with <see cref="StripToStudyNumberConverter"/>
        /// </summary>
        public static readonly DependencyProperty Group2Property =
            DependencyProperty.Register(nameof(Group2), typeof(string), typeof(StripsMenu), new PropertyMetadata("."));

        public static readonly DependencyProperty Group2ConverterProperty =
            DependencyProperty.Register(nameof(Group2Converter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToStudyNumberConverter()));

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register(nameof(ImageSize), typeof(double), typeof(StripsMenu), new PropertyMetadata(100.0));

        //public static readonly DependencyProperty ImageConverterProperty =
        //    DependencyProperty.Register(nameof(ImageConverter), typeof(IValueConverter), typeof(StripsMenu), new PropertyMetadata(new StripToImageConverter()));

        public static readonly DependencyProperty StripsProperty =
            DependencyProperty.Register(nameof(Strips), typeof(IEnumerable<StripVm>), typeof(StripsMenu), new PropertyMetadata(null));

        //public static readonly DependencyProperty StripActionsHolderProperty =
        //    DependencyProperty.RegisterAttached(nameof(StripActionsHolder), typeof(IStripActionsHolder), typeof(StripsMenu), new PropertyMetadata(null));


        public string DragNDrop { get; set; } = "StripDnD";

        public StripsMenu()
        {
            InitializeComponent();
            Loaded += StripsMenu_Loaded;
        }

        //public DataTemplate StripTemplate
        //{
        //    get { return (DataTemplate)GetValue(StripTemplateProperty); }
        //    set { SetValue(StripTemplateProperty, value); }
        //}
        public DataTemplateSelector StripTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(StripTemplateSelectorProperty); }
            set { SetValue(StripTemplateSelectorProperty, value); }
        }

        public string Group1
        {
            get { return (string)GetValue(Group1Property); }
            set { SetValue(Group1Property, value); }
        }

        public string Group2
        {
            get { return (string)GetValue(Group2Property); }
            set { SetValue(Group2Property, value); }
        }

        public IValueConverter Group1Converter
        {
            get { return (IValueConverter)GetValue(Group1ConverterProperty); }
            set { SetValue(Group1ConverterProperty, value); }
        }

        public IValueConverter Group2Converter
        {
            get { return (IValueConverter)GetValue(Group2ConverterProperty); }
            set { SetValue(Group2ConverterProperty, value); }
        }

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        //public IValueConverter ImageConverter
        //{
        //    get { return (IValueConverter)GetValue(ImageConverterProperty); }
        //    set { SetValue(ImageConverterProperty, value); }
        //}

        public IEnumerable<IStrip> Strips
        {
            get { return (IEnumerable<IStrip>)GetValue(StripsProperty); }
            set { SetValue(StripsProperty, value); }
        }

        //public IStripActionsHolder StripActionsHolder
        //{
        //    get { return (IStripActionsHolder)GetValue(StripActionsHolderProperty); }
        //    set { SetValue(StripActionsHolderProperty, value); }
        //}

        private void StripsMenu_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeStripGroups();
            //if (DataContext is IStripActionsHolder && StripActionsHolder == null)
            //    StripActionsHolder = DataContext as IStripActionsHolder;
        }

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
    }
}

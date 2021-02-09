using System;
using System.Collections.Generic;
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

namespace Ws.Fus.ImageViewer.UI.Wpf
{
    public enum ValuePosition
    {
        OneLine,
        TwoLines,
    };

    /// <summary>
    /// Interaction logic for StepProgress.xaml
    /// </summary>
    public partial class StepProgress : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ValuePositionProperty = DependencyProperty.Register("ValuePosition",
            typeof(ValuePosition), typeof(StepProgress), new UIPropertyMetadata(ValuePosition.TwoLines, OnValuePositionChanged));

        public ValuePosition ValuePosition
        {
            get { return (ValuePosition)GetValue(ValuePositionProperty); }
            set { SetValue(ValuePositionProperty, value); }
        }
        private static void OnValuePositionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StepProgress fe = sender as StepProgress;
            if (fe == null)
                return;

            fe.SetValuePosition();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(StepProgress), new UIPropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(string), typeof(StepProgress), new UIPropertyMetadata(null));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        public StepProgress()
        {
            InitializeComponent();
        }

        private void SetValuePosition()
        {
            switch (ValuePosition)
            {
                case ValuePosition.OneLine:
                    ValueTxt.SetValue(Grid.RowProperty, 0);
                    ValueTxt.SetValue(Grid.ColumnProperty, 2);
                    break;
                case ValuePosition.TwoLines:
                default:
                    ValueTxt.SetValue(Grid.RowProperty, 1);
                    ValueTxt.SetValue(Grid.ColumnProperty, 1);
                    break;
            }
        }

    }
}

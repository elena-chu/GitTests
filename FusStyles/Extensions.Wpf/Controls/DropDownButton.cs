using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class DropDownButton : ToggleButton
    {
        public DropDownButton()
        {
            // Bind ToggleButton.IsChecked property to DropDown.IsOpen
            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            this.SetBinding(IsCheckedProperty, binding);
        }

        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));



        public PlacementMode DropDownPlacement
        {
            get { return (PlacementMode)GetValue(DropDownPlacementProperty); }
            set { SetValue(DropDownPlacementProperty, value); }
        }
        public static readonly DependencyProperty DropDownPlacementProperty = DependencyProperty.Register("DropDownPlacement", typeof(PlacementMode), typeof(DropDownButton), new PropertyMetadata(PlacementMode.Bottom));


        // Overrides
        protected override void OnClick()
        {
            if (DropDown != null)
            {
                // If DropDown assigned to button, then position and display it
                DropDown.PlacementTarget = this;
                DropDown.Placement = DropDownPlacement;
                DropDown.IsOpen = true;
            }
        }
    }
}

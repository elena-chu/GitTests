using System.Linq;
using System.Windows.Controls;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public partial class ToolbarMenu : UserControl
    {
        public ToolbarMenu()
        {
            InitializeComponent();
        }

        private void OnToolbarMenuHeaderActivated(ToolbarMenuHeader menuHeader)
        {
            if (menuHeader.IsActive)
                Menu.Items.Cast<ToolbarMenuHeader>().Where(x => !x.Equals(menuHeader)).ToList().ForEach(x => x.InactivateExclusiveGroupMember());
        }
    }
}

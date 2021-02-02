using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Ws.Fus.ImageViewer.UI.Wpf;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;
using Ws.Fus.Treatment.UI.Wpf.Views;

namespace Ws.Fus.Treatment.UI.Wpf
{
    /// <summary>
    /// Main Screen available actions
    /// </summary>
    public enum MainScreenAction
    {
        Treatment = 0,
        DQA,
        Screening,
        PrePlanning,
        DataBase,
        Settings
    }
}

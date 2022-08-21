using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ws.Fus.Fge.Interfaces;
using Ws.Fus.Fge.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Entities
{
	public class GraphicEngineEventInfo 
	{
		public GraphicEngineMouseEvent EventType;
		public GraphicEngineMouseButton MouseButton;
        /// <summary>
        /// In pixels. Relative to current window.
        /// </summary>
		public int MousePosX;
		public int MousePosY;
        /// <summary>
        /// mouse wheel delta for MouseWheel events (can be something else for other new events)
        /// </summary>
		public int Amount; 

		public double RelativeCoeffPosX;
        public double RelativeCoeffPosY;
    }
}

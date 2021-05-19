using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.ImageViewer.Interfaces.Entities
{
    public enum CompareMode
    {
        /// <summary>
        /// Single Image
        /// </summary>
        Simple = 0,
        /// <summary>
        /// Shows two images alternately.
        /// </summary>
        Flicker,
        /// <summary>
        /// Shows two images simultaneously, while one image in the left part of the window and another in the right part.
        /// </summary>
        Slider,
        /// <summary>
        /// Blend between two images.
        /// </summary>
        Fusion
    }    
}

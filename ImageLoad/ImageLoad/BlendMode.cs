using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLoad
{
    public enum BlendMode
    {
        /// <summary>
        /// Alpha blending uses the alpha channel to combine the source and destination. 
        /// </summary>
        Alpha,

        /// <summary>
        /// Additive blending adds the colors of the source and the destination.
        /// </summary>
        Additive,

        /// <summary>
        /// Subtractive blending subtracts the source color from the destination.
        /// </summary>
        Subtractive,

        /// <summary>
        /// Uses the source color as a mask.
        /// </summary>
        Mask,

        /// <summary>
        /// Multiplies the source color with the destination color.
        /// </summary>
        Multiply,

        /// <summary>
        /// Ignores the specified Color
        /// </summary>
        ColorKeying,

        /// <summary>
        /// No blending just copies the pixels from the source.
        /// </summary>
        None
    }
}

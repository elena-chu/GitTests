using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageLoad
{
    public class ImageData
    {
        public byte[] Array { get; set; } 
        public int BytesPerPixel { get; set; }
        public Color AddedColor { get; set; } = Colors.White;
        public bool UseAlfa { get; set; }

        public bool IsImadedataValid()
        {
            return Array != null && Array.Length > 0;
        }
    }
}

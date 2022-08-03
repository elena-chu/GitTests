using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class SoundsHelper
    {
        public static void PlaySound(Stream sound)
        {
            SoundPlayer soundPlayer = new SoundPlayer(sound);
            soundPlayer.Play();
        }
    }
}

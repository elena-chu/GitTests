using System.IO;
using System.Media;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class SoundsHelper
    {
        public static void PlaySound(Stream sound, bool isEmergencySound = false)
        {
            SoundPlayer soundPlayer = new SoundPlayer(sound);
            if (isEmergencySound || !MuteAllSoundsExceptEmergency)
                soundPlayer.Play();
        }

        public static bool MuteAllSoundsExceptEmergency { get; set; } = false;
    }
}

using System.IO;
using System.Media;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class SoundsHelper
    {
        public static void PlaySound(Stream sound, bool isEmergencySound = false)
        {
            SoundPlayer soundPlayer = new SoundPlayer(sound);
            if (!MuteSounds)
                soundPlayer.Play();
            else if (isEmergencySound)
            {
                AudioManager.SetMasterVolumeMute(true);
                soundPlayer.Play();
                AudioManager.SetMasterVolumeMute(false);
            }
        }

        public static void ToggleMuteAllSoundsExceptEmergency()
        {
            MuteSounds = !MuteSounds;
            AudioManager.SetMasterVolumeMute(MuteSounds);
        }

        public static bool MuteSounds { get; private set; } = false;
    }
}

using System.IO;
using System.Media;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class SoundsHelper
    {
        public static void PlaySound(string soundFileName, bool isEmergencySound = false)
        {
            if (!MuteSounds)
                NAudioHelper.Play(soundFileName);
            else if (isEmergencySound)
            {
                AudioManager.SetMasterVolumeMute(true);
                NAudioHelper.Play(soundFileName);
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

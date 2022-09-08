using NAudio.Wave;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class NAudioHelper
    {
        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;

        public static void Play(string soundFileName)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(soundFileName);
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }

        private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }
    }
}

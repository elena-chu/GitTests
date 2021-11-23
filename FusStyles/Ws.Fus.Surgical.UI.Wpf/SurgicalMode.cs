namespace Ws.Fus.Surgical.UI.Wpf
{
    public enum SurgicalMode
    {
        Definition,
        Sonication,
        Dosimetry,
        Playback
    }

    public enum SonicateState
    {
        CoolingRunning,
        CoolingComplete,
        SonicateReady,
        SonicatePress,
        SonicateDisabled
    }
}

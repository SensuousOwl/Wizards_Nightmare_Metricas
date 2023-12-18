namespace _Main.Scripts.Audio
{
    public interface ISfxAudioPlayer
    {
        bool TryPlayRequestedClip(string p_clipID, float p_volume=1);
    }
}
using UnityEngine;

namespace _Main.Scripts.Audio
{
    public interface ISfxAudioPlayer
    {
        bool TryPlayRequestedClip(string p_clipID, float p_volume=1);
        public void ForcePlayOneShotClip(AudioClip p_audioClip, float p_volume = 1);
    }
}
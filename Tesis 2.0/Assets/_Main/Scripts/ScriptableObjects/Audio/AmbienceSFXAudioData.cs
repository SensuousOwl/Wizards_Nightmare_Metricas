using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Main/Audio/AmbienceSFXAudioPool")]
    public class AmbienceSFXAudioData : ScriptableObject
    {
        public AudioClip[] ambienceSfxClips;
    }
}
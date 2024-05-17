using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Main/Audio/AmbienceSFXAudioPool")]
    public class AmbienceSfxAudioData : ScriptableObject
    {
        public AudioClip[] ambienceSfxClips;
    }
}
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "PsychoKitties/Audio/AmbienceSFXAudioPool")]
    public class AmbienceSFXAudioData : ScriptableObject
    {
        public AudioClip[] ambienceSfxClips;
    }
}
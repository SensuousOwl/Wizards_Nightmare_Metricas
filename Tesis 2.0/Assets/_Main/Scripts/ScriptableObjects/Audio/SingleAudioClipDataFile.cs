using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Main/Audio/AudioClipSingle")]
    public sealed class SingleAudioClipDataFile : AudioClipDataFile
    {
        public AudioClip clipData;

        public override AudioClip GetAudioClip()
        {
            return clipData;
        }
    }
}
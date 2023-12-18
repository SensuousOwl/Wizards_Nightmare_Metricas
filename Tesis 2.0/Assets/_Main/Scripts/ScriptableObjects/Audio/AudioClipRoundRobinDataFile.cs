using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Main/Audio/AudioClipRoundRobin")]
    public sealed class AudioClipRoundRobinDataFile : AudioClipDataFile
    {
        public AudioClip[] roundRobins;

        public override AudioClip GetAudioClip()
        {
            return roundRobins.Length < 1 ? default : roundRobins.GetRandomElement();
        }
    }
}
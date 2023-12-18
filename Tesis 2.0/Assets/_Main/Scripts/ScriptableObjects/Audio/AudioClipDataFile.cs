using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    public abstract class AudioClipDataFile : ScriptableObject
    {
        public string clipID;
        public abstract AudioClip GetAudioClip();
    }
}
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Main/Audio/FixedBGM")]
    public class FixedBGMPool : ScriptableObject
    {
        public  AudioClip intro, loop;
    }
}
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "PsychoKitties/Audio/FixedBGM")]
    public class FixedBGMPool : ScriptableObject
    {
        public  AudioClip intro, loop;
    }
}
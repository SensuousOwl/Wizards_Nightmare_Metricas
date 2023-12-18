using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "PsychoKitties/Audio/AudioPoolData")]
    public class AudioPoolData : ScriptableObject
    {
        [SerializeField] private AudioClipDataFile[] audioClipFiles;

        public AudioClip TryGetAudioClipWithID(string p_clipNameID)
        {
            for (int i = 0; i < audioClipFiles.Length; i++)
            {
                if (audioClipFiles[i].clipID == p_clipNameID)
                {
                    return audioClipFiles[i].GetAudioClip();
                }
            }

            return default;
        }
    }
}
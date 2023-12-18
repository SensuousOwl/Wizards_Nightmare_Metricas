using _Main.Scripts.ScriptableObjects.Audio;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Audio
{
    public class SfxAudioPoolPlayer : MonoBehaviour, ISfxAudioPlayer
    {
        [SerializeField] private AudioPoolData audioPoolData;

        private static IEventService EventService => ServiceLocator.Get<IEventService>();

        /// <summary>
        /// Tries to play the requested audio clip, if found
        /// </summary>
        /// <param name="p_clipID">AudioClip ID to find</param>
        /// <returns>if the request was successful</returns>
        public bool TryPlayRequestedClip(string p_clipID, float p_volume=1)
        {
            if (audioPoolData == default)
                return false;
            var l_audioClip = audioPoolData.TryGetAudioClipWithID(p_clipID);
            var l_isValidClip = l_audioClip != default;
            if (l_isValidClip)
            {
                EventService.DispatchEvent(new PlayEffectSound(l_audioClip,p_volume));
            }

            return l_isValidClip;
        }

        public void ForcePlayOneShotClip(AudioClip p_audioClip,  float p_volume=1)
        {
            if (p_audioClip == default)
                return;
            EventService.DispatchEvent(new PlayEffectSound(p_audioClip, p_volume));
        }
    }
}
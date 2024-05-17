using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct PlayEffectSoundEventData : ICustomEventData
    {
        public AudioClip AudioClip { get; }
        public float Volume { get; }
        
        public PlayEffectSoundEventData(AudioClip p_audioClip, float p_v=1)
        {
            AudioClip = p_audioClip;
            Volume = p_v;
        }
    }
}
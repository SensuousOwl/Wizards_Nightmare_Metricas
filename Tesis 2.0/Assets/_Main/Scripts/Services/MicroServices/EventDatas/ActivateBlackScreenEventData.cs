using System;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    [Serializable]
    public struct ActivateBlackScreenEventData : ICustomEventData
    {
        [field: SerializeField] public float SecondsInActive { get; private set; }
        [field: SerializeField] public float DelayInActive { get; private set; }

        public Action OnScreenStarted { get; }
        public Action OnScreenCompleted { get; }
        public Action OnScreenFinished { get; }
        
        public ActivateBlackScreenEventData(float p_secondsInActive = 1f, float p_delayInActive = 0f, Action p_onScreenStarted = default, Action p_onScreenCompleted = default, Action p_onScreenFinished = default)
        {
            OnScreenStarted = p_onScreenStarted;
            OnScreenCompleted = p_onScreenCompleted;
            OnScreenFinished = p_onScreenFinished;
            SecondsInActive = p_secondsInActive;
            DelayInActive = p_delayInActive;
        }
    }
}
using System;
using System.Collections;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;
using UnityEngine.UI;

namespace _main.Scripts.Managers
{
    public class BlackScreenManager : MonoBehaviour
    {
        [SerializeField] private float delayTimer = 1.5f;
        [SerializeField] private Image blackImage;

        private bool m_screenOnFade;
        
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        
        private void OnEnable()
        {
            EventService.AddListener<ActivateBlackScreenEventData>(ActiveBlackScreen);
        }

        private void OnDisable()
        {
            EventService.RemoveListener<ActivateBlackScreenEventData>(ActiveBlackScreen);
        }
        
        private void ActiveBlackScreen(ActivateBlackScreenEventData p_data)
        {
            if (m_screenOnFade)
                return;
            
            StartCoroutine(ScreenFadeCoroutine(p_data));
        }
        
        private IEnumerator ScreenFadeCoroutine(ActivateBlackScreenEventData p_data)
        {
            yield return new WaitForSeconds(p_data.DelayInActive);
            p_data.OnScreenStarted?.Invoke();
            m_screenOnFade = true;
            var l_timer = delayTimer;
            var l_newColor = blackImage.color;

            while (l_timer > 0)
            {
                l_timer -= Time.deltaTime;
                l_newColor.a += Time.deltaTime;
                blackImage.color = l_newColor;
                yield return null;
            }

            l_newColor.a = 1;
            blackImage.color = l_newColor;

            l_timer = delayTimer;
            
            p_data.OnScreenCompleted?.Invoke();
            
            yield return new WaitForSeconds(p_data.SecondsInActive);

            while (l_timer > 0)
            {
                l_timer -= Time.deltaTime;
                l_newColor.a -= Time.deltaTime;
                blackImage.color = l_newColor;
                yield return null;
            }

            l_newColor.a = 0;
            blackImage.color = l_newColor;
            m_screenOnFade = false;
            p_data.OnScreenFinished?.Invoke();
        }

#if UNITY_EDITOR
        [Header("Only Editor")] 
        [SerializeField] private ActivateBlackScreenEventData testData;

        [ContextMenu("TestBlackScreen")]
        private void TestBlackScreen()
        {
            StartCoroutine(ScreenFadeCoroutine(testData));
        }
#endif
    }

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
using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.UI
{
    public class ShowCurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text Text;
        [SerializeField] private float changeTime=2;
        [SerializeField] private float fadeTime;

        
        private static ICurrencyService CurrencyService => ServiceLocator.Get<ICurrencyService>();
        private int m_currentValue;
        private void Start()
        {
            CurrencyService.OnCurrencyChange += OnCurrencyChange;
        }

        private void OnCurrencyChange(int p_obj)
        {
            StartCoroutine(ChangeValueGradually(p_obj));
        }
/*
        private void OnCurrencyChange(int p_newValue)
        {
            Text.alpha = 255;
            if (m_currentValue < p_newValue)
            {
                while (m_currentValue <= p_newValue)
                {
                    Text.text = m_currentValue.ToString();
                    m_currentValue++;
                }

                m_currentValue = p_newValue;
            }
            else
            {
                while (m_currentValue >= p_newValue)
                {
                    Text.text = m_currentValue.ToString();
                    m_currentValue--;
                }
                m_currentValue = p_newValue;
            }

            while (Text.alpha<0)
            {
                Text.alpha -= Text.alpha * Time.deltaTime * fadeTime;
            }
            
        }
        */
        private IEnumerator ChangeValueGradually(int p_newValue)
        {
            float l_elapsedTime = 0f;
            int l_startingValue = m_currentValue;

            while (l_elapsedTime < changeTime)
            {
                l_elapsedTime += Time.deltaTime;
                float l_t = l_elapsedTime / changeTime;

                
                m_currentValue = (int)Mathf.Lerp(l_startingValue, p_newValue, l_t);
                Text.text = Mathf.RoundToInt(m_currentValue).ToString();

                yield return null;
            }

            
            m_currentValue = p_newValue;
            Text.text = m_currentValue.ToString();

            
            StartCoroutine(FadeOutText());
        }

        private IEnumerator FadeOutText()
        {
            float l_elapsedTime = 0f;

            while (l_elapsedTime < fadeTime)
            {
                l_elapsedTime += Time.deltaTime;
                float l_t = l_elapsedTime / fadeTime;

                Text.alpha = Mathf.Lerp(1f, 0f, l_t);
                yield return null;
            }

            Text.alpha = 0f;
        }
        
    }
}
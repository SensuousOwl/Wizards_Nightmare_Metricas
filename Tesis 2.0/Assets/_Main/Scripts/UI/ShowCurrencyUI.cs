using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class ShowCurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private TMP_Text gsText;
        [SerializeField] private Image background;
        [SerializeField] private Image borders;
        [SerializeField] private float changeTime=2;
        [SerializeField] private float fadeTime;

        
        private static ICurrencyService CurrencyService => ServiceLocator.Get<ICurrencyService>();
        private int m_currentValue;
        private void Start()
        {
            CurrencyService.OnCurrencyChange += OnCurrencyChange;
            OnCurrencyChange(CurrencyService.GetCurrentGs());
        }

        private void OnDestroy()
        {
            CurrencyService.OnCurrencyChange -= OnCurrencyChange;
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
                numberText.text = Mathf.RoundToInt(m_currentValue).ToString();

                yield return null;
            }
            
            m_currentValue = p_newValue;
            numberText.text = m_currentValue.ToString();
            
            StartCoroutine(FadeOutText());
            yield return null;
        }

        private IEnumerator FadeOutText()
        {
            float l_elapsedTime = 0f;

            while (l_elapsedTime < fadeTime)
            {
                l_elapsedTime += Time.deltaTime;
                float l_t = l_elapsedTime / fadeTime;

                numberText.alpha = Mathf.Lerp(1f, 0f, l_t);
                gsText.alpha = Mathf.Lerp(1f, 0f, l_t);
                background.color=new Color(background.color.r,background.color.g,background.color.b,Mathf.Lerp(1f, 0f, l_t));
                borders.color=new Color(background.color.r,background.color.g,background.color.b,Mathf.Lerp(1f, 0f, l_t));
                yield return null;
            }

            numberText.alpha = 0f;
            gsText.alpha = 0f;
            background.color=new Color(background.color.r,background.color.g,background.color.b,0);
            borders.color=new Color(background.color.r,background.color.g,background.color.b,0);
            yield return null;
        }
        
    }
}
using System;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.UI
{
    public class ShowCurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text Text;

        private ICurrencyService m_currencyService => ServiceLocator.Get<ICurrencyService>();
        private void Start()
        {
            m_currencyService.OnCurrencyChange += OnCurrencyChange;
        }

        private void OnCurrencyChange(int p_obj)
        {
            Text.text = p_obj.ToString();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using UnityEngine;

public class AddCurrency : MonoBehaviour
{
    [SerializeField] private int addCurrency;
    private ICurrencyService m_currencyService = ServiceLocator.Get<ICurrencyService>();

    private void Start()
    {
        m_currencyService.AddGs(addCurrency);
    }

#if UNITY_EDITOR

    [ContextMenu("AddCurrency")]

    void AddCurrencyInGame()
    {
        m_currencyService.AddGs(addCurrency);
    }
    
#endif
}

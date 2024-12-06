using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System;

public class AnalyticsInitializer : MonoBehaviour
{
    async void Start()
    {
        try
        {
            Debug.Log("Inicializando Unity Services...");
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services inicializado correctamente.");
         
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log($"Consent given! We can get the data ! ! !");
    }
}

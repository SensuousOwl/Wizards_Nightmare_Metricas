using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using TMPro;

public class AnalyticsManager_ : MonoBehaviour
{
    private async void Start()
    {
        // Inicializamos Unity Services
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Services y Analytics inicializados correctamente.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error inicializando Unity Services: {e.Message}");
        }
    }
}
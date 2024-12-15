using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;

public class FPSMonitor : MonoBehaviour
{
    [Header("FPS Settings")]
    [SerializeField] private float criticalFPSThreshold = 60f; // Umbral cr�tico
    private float currentFPS;

    private string currentScene;

   
    private async void Start()
    {
        // Inicializar Unity Services
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

        // Obtener el nombre de la escena actual
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        // Calcular los FPS actuales
        currentFPS = 1.0f / Time.deltaTime;

        // Si los FPS caen por debajo del umbral, enviar el evento
        if (currentFPS < criticalFPSThreshold)
        {
            SendLowFPSEvent();
        }
    }

    private void SendLowFPSEvent()
    {
        try
        {
            AnalyticsService.Instance.CustomData("Low_FPS", new Dictionary<string, object>
            {
                { "CurrentFPS", Mathf.Round(currentFPS) }, // Redondear FPS para claridad
                { "SceneName", currentScene }
            });

            AnalyticsService.Instance.Flush(); // Forzar env�o inmediato para pruebas
            Debug.Log($"Evento Low_FPS enviado: CurrentFPS = {currentFPS}, SceneName = {currentScene}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error enviando evento Low_FPS: {e.Message}");
        }
    }
}



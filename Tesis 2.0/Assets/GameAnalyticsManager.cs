using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using TMPro;

public class GameAnalyticsManager : MonoBehaviour
{
    public Button skipButton; // Asigna el bot�n desde el Inspector
    public TextMeshProUGUI dialogueText; // El texto del di�logo (opcional, si quieres mostrar algo)
    private int spamCount = 0; // Contador de clics en el bot�n de salto
    private float lastClickTime = 0f; // Tiempo del �ltimo clic
    private float spamThreshold = 0.5f; // Tiempo m�nimo entre clics para considerarlo spam (en segundos)

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

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(TrackSpamClick);
        }
    }

    private void TrackSpamClick()
    {
        float currentTime = Time.time;

        // Comprobar si el clic es dentro del umbral para considerarlo spam
        if (currentTime - lastClickTime < spamThreshold)
        {
            spamCount++;
        }
        lastClickTime = currentTime;

        // Enviar el evento con CustomData
        try
        {
            AnalyticsService.Instance.CustomData("Dialogue_Skip_Spam", new Dictionary<string, object>
            {
                { "SpamCount", spamCount }, // N�mero de clics r�pidos
                { "TotalClicks", spamCount + 1 }, // Total de clics (incluyendo los normales)
                { "TimeSinceStart", Time.time } // Tiempo desde el inicio del juego
            });

            AnalyticsService.Instance.Flush(); // Forzar env�o inmediato para pruebas
            Debug.Log($"Evento enviado: Dialogue_Skip_Spam - SpamCount: {spamCount}, TotalClicks: {spamCount + 1}");


        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error enviando evento: {e.Message}");
        }
    }

    private void OnDestroy()
    {
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(TrackSpamClick);
        }
    }
}

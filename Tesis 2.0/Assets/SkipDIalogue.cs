using UnityEngine;
using UnityEngine.UI; // Para gestionar el bot�n
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;

public class SkipDialogueManager : MonoBehaviour
{
   public void LogDialogueSkipped(string dialogueName)
    {
        try
        {
            // Enviar evento usando CustomData
            AnalyticsService.Instance.CustomData("Dialogue_Skipped", new System.Collections.Generic.Dictionary<string, object>
            {
                { "DialogueName", dialogueName }, // Nombre del di�logo omitido
                { "TimeSkipped", Time.time } // Tiempo en segundos desde el inicio del juego
            });

            // Forzar el env�o inmediato
            AnalyticsService.Instance.Flush();

            Debug.Log($"Evento enviado correctamente: Dialogue_Skipped - {dialogueName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error enviando evento: {e.Message}");
        }
    }
}
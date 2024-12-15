using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public static class MenuTimeTracker
{
    private static float openTime;
    private static string currentScreen;

    public static void StartTracking(string screenName)
    {
        openTime = Time.time;
        currentScreen = screenName;
        Debug.Log($"Iniciando tracking para {screenName} a las {openTime}");
    }

    public static void StopAndSendAnalytics()
    {
        if (string.IsNullOrEmpty(currentScreen)) return;

        float duration = Time.time - openTime;
        Debug.Log($"Cerrando {currentScreen}. Duración: {duration} segundos.");

        // Enviar el evento a Unity Analytics
        AnalyticsService.Instance.CustomData("Menu_Screen_Time", new Dictionary<string, object>
        {
            { "ScreenType", currentScreen },
            { "Duration", duration }
        });

        AnalyticsService.Instance.Flush();
        currentScreen = null; // Resetear
    }
}

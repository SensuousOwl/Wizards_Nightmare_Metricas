using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using TMPro;

public class GameAnalyticsManager : MonoBehaviour
{
    public Button skipButton;
    public TextMeshProUGUI dialogueText;
    private int spamCount = 0;
    private float lastClickTime = 0f;
    private float spamThreshold = 0.5f;

    private async void Start()
    {
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
        if (currentTime - lastClickTime < spamThreshold)
        {
            spamCount++;
        }
        lastClickTime = currentTime;

        try
        {
            AnalyticsService.Instance.CustomData("DialogueSkipSpam", new Dictionary<string, object>
            {
                { "Spaming_Count", spamCount },
                { "Totalde_Clicks", spamCount + 1 }
            });

            Debug.Log($"Evento enviado: Dialogue_Skip_Spam - Spaming_Count: {spamCount}, Totalde_Clicks: {spamCount + 1}");
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

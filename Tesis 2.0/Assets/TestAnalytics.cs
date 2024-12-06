using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class TestAnalytics : MonoBehaviour
{
    void Start()
    {
        AnalyticsResult result = Analytics.CustomEvent("test_event", new Dictionary<string, object>
        {
            { "test_param", "value" },
            { "player_level", 1 }
        });

        Debug.Log("Resultado del envío de evento: " + result);
    }
}

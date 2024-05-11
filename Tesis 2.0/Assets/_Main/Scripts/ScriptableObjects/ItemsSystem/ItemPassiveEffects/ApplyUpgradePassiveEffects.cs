using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/ApplyUpgrade")]
    public class ApplyUpgradePassiveEffects : ItemPassiveEffect
    {
        [System.Serializable]
        private struct MyData
        {
            [field: SerializeField] public StatsId StatId { get; private set; }
            [field: SerializeField] public bool SubtractValue { get; private set; }
            [field: SerializeField] public bool ForPercentage { get; private set; }
            [field: SerializeField] public float Value { get; private set; }
        }
        [SerializeField] private List<MyData> data;
        private readonly Dictionary<StatsId, float> m_dictionary = new();

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        
        public override void Activate()
        {
            foreach (var l_data in data)
            {
                float l_value;
                if (l_data.ForPercentage)
                    l_value = StatsService.GetStatById(l_data.StatId) * l_data.Value / 100;
                else
                    l_value = l_data.Value;
                
                if (!l_data.SubtractValue)
                    StatsService.AddUpgradeStat(l_data.StatId, l_value);
                else
                    StatsService.SubtractUpgradeStat(l_data.StatId, l_value);
                
                m_dictionary.Add(l_data.StatId, l_value);
            }
        }

        public override void Deactivate()
        {
            foreach (var l_data in data)
            {
                if (m_dictionary.TryGetValue(l_data.StatId, out var l_value))
                    continue;
                
                if (!l_data.SubtractValue)
                    StatsService.SubtractUpgradeStat(l_data.StatId, l_value);
                else
                    StatsService.AddUpgradeStat(l_data.StatId, l_value);
            }
        }
    }
}
using System.Collections.Generic;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/ApplyUpgrade")]
    public class ApplyUpgradePassiveEffect : ItemPassiveEffect
    {
        [System.Serializable]
        private struct MyData
        {
            [field: SerializeField] public StatsId StatId { get; private set; }
            [field: SerializeField] public bool SubtractValue { get; private set; }
            [field: SerializeField] public bool ForPercentage { get; private set; }
            [field: SerializeField] public float Value { get; private set; }
        }

        private struct DelegateUpgradeData
        {
            public StatsId StatsID { get; private set; }
            public float Value { get; private set; }
            public bool SubtractValue { get; private set; }
            

            public DelegateUpgradeData(StatsId p_statsID, float p_value, bool p_subtractValue)
            {
                StatsID = p_statsID;
                Value = p_value;
                SubtractValue = p_subtractValue;
            }
        }
        
        
        
        [SerializeField] private List<MyData> config;
        private readonly List<DelegateUpgradeData> m_list = new();

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        
        public override void Activate()
        {
            foreach (var l_data in config)
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
                
                m_list.Add(new DelegateUpgradeData(l_data.StatId, l_value, l_data.SubtractValue));
            }
        }

        public override void Deactivate()
        {
            foreach (var l_data in m_list)
            {
                if (!l_data.SubtractValue)
                    StatsService.SubtractUpgradeStat(l_data.StatsID, l_data.Value);
                else
                    StatsService.AddUpgradeStat(l_data.StatsID, l_data.Value);
            }
            
            m_list.Clear();
        }
    }
}
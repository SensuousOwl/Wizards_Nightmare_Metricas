using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/ApplyUpgrade")]
    public class ApplyUpgradeActiveEffect : ItemActiveEffect
    {
        [System.Serializable]
        private struct MyData
        {
            [field: SerializeField] public StatsId StatId { get; private set; }
            [field: SerializeField] public bool SubtractValue { get; private set; }
            [field: SerializeField] public bool ForPercentage { get; private set; }
            [field: SerializeField] public bool IsPermanent { get; private set; }
            [field: SerializeField] public float Value { get; private set; }
            [field: SerializeField] public float TimeInUse { get; private set; }
        }
        
        [SerializeField] private List<MyData> config;

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        public override void UseItem()
        {
            foreach (var l_data in config)
            {
                if (!l_data.IsPermanent)
                    PlayerModel.Local.StartCoroutine(UseEffectCoroutine(l_data));
                else
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
                }
            }
        }

        private static IEnumerator UseEffectCoroutine(MyData p_data)
        {
            float l_value;
            if (p_data.ForPercentage)
                l_value = StatsService.GetStatById(p_data.StatId) * p_data.Value / 100;
            else
                l_value = p_data.Value;
                
            if (!p_data.SubtractValue)
                StatsService.AddUpgradeStat(p_data.StatId, l_value);
            else
                StatsService.SubtractUpgradeStat(p_data.StatId, l_value);
            
            yield return new WaitForSeconds(p_data.TimeInUse);
            
            if (p_data.SubtractValue)
                StatsService.AddUpgradeStat(p_data.StatId, l_value);
            else
                StatsService.SubtractUpgradeStat(p_data.StatId, l_value);
        }
    }
}
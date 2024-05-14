using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/RangeUpgrade")]
    public class RangeUpgradeEffect : UpgradeEffect
    {
        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        
        public override void ApplyEffect(float p_valuePercentage)
        {
            StatsService.AddUpgradeStatForPercentage(StatsId.Range, p_valuePercentage);
        }
    }
}
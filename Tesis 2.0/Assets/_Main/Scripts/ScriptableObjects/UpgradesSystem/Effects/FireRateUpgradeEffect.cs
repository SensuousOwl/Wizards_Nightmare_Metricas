using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/FireRateUpgrade")]
    public class FireRateUpgradeEffect : UpgradeEffect
    {
        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();

        public override void ApplyEffect(float p_valuePercentage)
        {
            StatsService.AddUpgradeStatForPercentage(StatsId.FireRate, p_valuePercentage);
        }
    }
}
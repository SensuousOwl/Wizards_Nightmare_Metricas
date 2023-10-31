using PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/ProjectileSpeedUpgrade")]
    public class ProjectileSpeedUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            p_model.StatsController.AddUpgradeStatForPercentage(StatsId.ProjectileSpeed, p_valuePercentage);
        }
    }
}
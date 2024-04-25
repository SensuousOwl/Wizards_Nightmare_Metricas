using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/ProjectileSpeedUpgrade")]
    public class ProjectileSpeedUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            PlayerModel.StatsController.AddUpgradeStatForPercentage(StatsId.ProjectileSpeed, p_valuePercentage);
        }
    }
}
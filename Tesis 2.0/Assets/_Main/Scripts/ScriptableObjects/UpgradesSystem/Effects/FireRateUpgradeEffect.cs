using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/FireRateUpgrade")]
    public class FireRateUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            PlayerModel.StatsController.AddUpgradeStatForPercentage(StatsId.FireRate, p_valuePercentage);
        }
    }
}
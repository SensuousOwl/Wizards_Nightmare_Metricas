using PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/MovementSpeedUpgrade")]
    public class MovementSpeedUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            p_model.StatsController.AddUpgradeStatForPercentage(StatsId.MovementSpeed, p_valuePercentage);
        }
    }
}
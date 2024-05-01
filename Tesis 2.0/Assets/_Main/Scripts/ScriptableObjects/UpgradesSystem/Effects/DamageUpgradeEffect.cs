using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/DamageUpgrade")]
    public class DamageUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            PlayerModel.StatsController.AddUpgradeStatForPercentage(StatsId.Damage, p_valuePercentage);
        }
    }
}
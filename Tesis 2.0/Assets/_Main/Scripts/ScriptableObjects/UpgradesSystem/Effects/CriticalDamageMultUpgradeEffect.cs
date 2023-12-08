using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/CriticalDamageMultUpgrade")]
    public class CriticalDamageMultUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            p_model.StatsController.AddUpgradeStatForPercentage(StatsId.CriticalDamageMult, p_valuePercentage);
        }
    }
}
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem.Effects
{
    [CreateAssetMenu(menuName = "Main/Upgrades/Effects/MaxLifeUpgrade")]
    public class MaxLifeUpgradeEffect : UpgradeEffect
    {
        public override void ApplyEffect(PlayerModel p_model, float p_valuePercentage)
        {
            var l_value = p_model.HealthController.GetMaxHealth() * (p_valuePercentage / 100);
            p_model.HealthController.AddMaxHealth(l_value);
        }
    }
}
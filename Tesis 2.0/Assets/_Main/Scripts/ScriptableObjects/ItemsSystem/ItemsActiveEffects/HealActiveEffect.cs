using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/HealActiveEffect")]
    public class HealActiveEffect : ItemActiveEffect
    {
        [SerializeField] private float healAmount;
        [SerializeField] private bool usePercentage;
        public override void UseItem()
        {
            if (!usePercentage)
            {
                PlayerModel.Local.HealthController.Heal(healAmount);
                return;
            }
            
            PlayerModel.Local.HealthController.HealPercentage(healAmount);
        }
    }
}
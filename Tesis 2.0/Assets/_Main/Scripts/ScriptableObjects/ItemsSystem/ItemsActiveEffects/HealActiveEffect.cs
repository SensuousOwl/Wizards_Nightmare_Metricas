using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/HealActiveEffect")]
    public class HealActiveEffect : ItemActiveEffect
    {
        [SerializeField] private float healAmount;
        public override void UseItem()
        {
            PlayerModel.Local.HealthController.Heal(healAmount);
        }
    }
}
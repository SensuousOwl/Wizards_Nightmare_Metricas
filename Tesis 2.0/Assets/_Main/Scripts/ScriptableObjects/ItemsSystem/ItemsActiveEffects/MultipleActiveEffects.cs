using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/MultipleActiveEffects")]
    public class MultipleActiveEffects : ItemActiveEffect
    {
        [SerializeField] private List<ItemActiveEffect> itemActiveEffects;
        
        public override void UseItem()
        {
            foreach (var l_effect in itemActiveEffects)
            {
                l_effect.UseItem();
            }
        }
    }
}
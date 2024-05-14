using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/MultiplePassiveEffects")]
    public class MultiplePassiveEffects : ItemPassiveEffect
    {
        [SerializeField] private List<ItemPassiveEffect> itemPassiveEffects;
        public override void Activate()
        {
            foreach (var l_effect in itemPassiveEffects)
            {
                l_effect.Activate();
            }
        }

        public override void Deactivate()
        {
            foreach (var l_effect in itemPassiveEffects)
            {
                l_effect.Deactivate();
            }
        }
    }
}
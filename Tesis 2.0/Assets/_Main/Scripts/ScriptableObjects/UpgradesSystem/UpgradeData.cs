using System.Collections.Generic;
using System.Linq;
using PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeData")]
    public class UpgradeData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public List<UpgradeEffect> Effects { get; private set; }
        [field: SerializeField] public List<float> UpgradePercentage { get; private set; }



        public void ApplyEffects(PlayerModel p_model)
        {

            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].ApplyEffect(p_model, UpgradePercentage[i]);
                
            }
        }
        
        #if UNITY_EDITOR
        
        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = Effects.Distinct().ToList();
            Effects = l_newList;

            if (Effects.Count != UpgradePercentage.Count)
            {
                while (Effects.Count > UpgradePercentage.Count)
                {
                    UpgradePercentage.Add(0);
                }

                while (Effects.Count < UpgradePercentage.Count)
                {
                    UpgradePercentage.RemoveAt(UpgradePercentage.Count - 1);
                }
            }

        }
        
        #endif
    }
}
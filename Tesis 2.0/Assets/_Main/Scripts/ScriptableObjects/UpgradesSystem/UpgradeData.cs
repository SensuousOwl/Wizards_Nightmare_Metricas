using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeData")]
    public class UpgradeData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }
        [field: SerializeField] public float UpgradeWeight { get; private set; }
        [field: SerializeField] public float UnlockWeight { get; private set; }
        [field: SerializeField] public List<UpgradeEffect> Effects { get; private set; }
        [field: SerializeField] public List<float> UpgradePercentage { get; private set; }
        [field: SerializeField] public Sprite BorderSprite { get; private set; }
        [field: SerializeField] public Sprite EffectSprite { get; private set; }


        public void ApplyEffects()
        {
            for (var l_i = 0; l_i < Effects.Count; l_i++)
            {
                Effects[l_i].ApplyEffect(UpgradePercentage[l_i]);
                Logger.Log("Apply upgrade: " + Effects[l_i]);
            }
        }
        
        #if UNITY_EDITOR
        
        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = Effects.Distinct().ToList();
            Effects = l_newList;

            if (Effects.Count == UpgradePercentage.Count) 
                return;
            
            while (Effects.Count > UpgradePercentage.Count)
            {
                UpgradePercentage.Add(0);
            }

            while (Effects.Count < UpgradePercentage.Count)
            {
                UpgradePercentage.RemoveAt(UpgradePercentage.Count - 1);
            }

        }
        

        [ContextMenu("SetRandomID")]
        private void SetRandomID()
        {
            Id = "RandomID" + Random.Range(0, 10000);
        }


        
        #endif
    }
}
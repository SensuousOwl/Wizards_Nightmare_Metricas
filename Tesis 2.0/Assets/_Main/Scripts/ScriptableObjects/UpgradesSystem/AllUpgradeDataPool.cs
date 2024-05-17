using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.DictionaryUtilities;
using UnityEngine;
using Logger = _Main.Scripts.StaticClass.Logger;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeDataPool")]
    public class AllUpgradeDataPool : ScriptableObject
    {
        [field: SerializeField]
        public SerializableDictionary<string, UpgradeData> UpgradesDataDictionary { get; private set; }

        [field: SerializeField] public List<string> DefaultUnlockedUpgrades { get; private set; }

#if UNITY_EDITOR

        [Header("EDITOR ONLY")] [SerializeField]
        private List<UpgradeData> upgradesData;

        [ContextMenu("FillDictionaryData")]
        private void FillData()
        {
            UpgradesDataDictionary.Clear();
            
            foreach (var l_element in upgradesData)
            {
                UpgradesDataDictionary.TryAdd(l_element.Id, l_element);
            }
        }
        
        [ContextMenu("CheckUnlockedUpgradesIds")]
        public void CheckUnlockedUpgradesIds()
        {
            for (int i = 0; i < DefaultUnlockedUpgrades.Count; i++)
            {
                var l_currId = DefaultUnlockedUpgrades[i];
                
                if(UpgradesDataDictionary.ContainsKey(l_currId))
                    continue;
                
                Logger.LogError($"Default Unlocked Upgrade ID was not found in upgrades Dictionary. ID not found was '{l_currId}' in i : {i}");
            }
        }


#endif
    }
}
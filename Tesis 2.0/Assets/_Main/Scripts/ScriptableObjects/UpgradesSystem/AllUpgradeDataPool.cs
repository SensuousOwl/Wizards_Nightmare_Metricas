using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using _Main.Scripts.Attributes;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    [Serializable]
    public struct UpgradeDataStruct
    {
        public string id;
        public UpgradeData data;
        public float weight;
    }
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeDataPool")]
    public class AllUpgradeDataPool : ScriptableObject
    {
        [field: SerializeField] public SerializableDictionary<string, UpgradeDataStruct> UpgradeDataStructs; 
        [SerializeField] private List<UpgradeData> upgrades;
        [SerializeField] private List<float> upgradesChances;

        
        public List<UpgradeData> AllUpgrades => upgrades;
        public List<float> AllUpgradesChances => upgradesChances;
        
        private RouletteWheel<UpgradeData> m_rouletteWheel;
        public UpgradeData GetRandomUpgradeFromPool()
        {
            m_rouletteWheel ??= new RouletteWheel<UpgradeData>(upgrades, upgradesChances);

            return m_rouletteWheel.RunWithCached();
        }
        
        public UpgradeData GetRandomUpgradeFromPool(List<UpgradeData> p_upgradesExclude)
        {
            m_rouletteWheel ??= new RouletteWheel<UpgradeData>(upgrades, upgradesChances);
            
            var l_upgrade = m_rouletteWheel.RunWithCached();
            var l_watchDog = 1000;

            while (p_upgradesExclude.Contains(l_upgrade) && l_watchDog > 0)
            {
                l_upgrade = m_rouletteWheel.RunWithCached();
                l_watchDog--;
            }

            return l_upgrade;
        }

        public UpgradeData GetDataById(string id)
        {
            if (!UpgradeDataStructs.TryGetValue(id, out var upgradeDataStruct))
                return default;

            return upgradeDataStruct.data;
        }

        public UpgradeDataStruct GetStructByData(UpgradeData data)
        {
            foreach (var element in UpgradeDataStructs)
            {
                if(!element.Value.data.Equals(data))
                    continue;

                return element.Value;
            }

            Debug.LogError("Error in getting struct via Data, data was not found");
            return default;
        }

#if UNITY_EDITOR
        
        [Header("Editor Only")]
        [ReadOnlyInspector, SerializeField] public List<float> chancePercentage = new();
        
        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = upgrades.Distinct().ToList();
            upgrades = l_newList;

            if (upgrades.Count != upgradesChances.Count)
            {
                while (upgrades.Count > upgradesChances.Count)
                {
                    upgradesChances.Add(0);
                }

                while (upgrades.Count < upgradesChances.Count)
                {
                    upgradesChances.RemoveAt(upgradesChances.Count - 1);
                }
            }

            CheckItemPercentage();
        }

        [ContextMenu("Show item percentage")]
        public void CheckItemPercentage()
        {
            var l_totalChance = upgradesChances.Sum();

            chancePercentage.Clear();

            foreach (var l_value in upgradesChances)
            {
                chancePercentage.Add((l_value / l_totalChance) * 100);
            }
        }
#endif
    }
}
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Attributes;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeDataPool")]
    public class UpgradeDataPool : ScriptableObject
    {
        [SerializeField] private List<UpgradeData> upgrades;
        [SerializeField] private List<float> upgradesChances;
        
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
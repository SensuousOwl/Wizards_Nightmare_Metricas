using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using UnityEngine;
using ListExtensions = _Main.Scripts.Extensions.ListExtensions;

namespace _Main.Scripts.Services.UpgradePoolServices
{
    public class UpgradePoolService : IUpgradePoolService
    {
        private AllUpgradeDataPool m_allAllUpgradeData;
        
        
        private List<string> m_unlockedUpgradeIDs;
        private List<string> m_lockedUpgradeIDs;
        
        private RouletteWheel<UpgradeDataStruct> m_unlockedUpgradesRouletteWheel;
        private RouletteWheel<UpgradeDataStruct> m_lockedUpgradesRouletteWheel;
        public void Initialize()
        {
            m_allAllUpgradeData = MyGame.AllUpgradePoolDataEssentials;
            RefreshUpgradeLists();
        }
        private void RefreshUpgradeLists()
        {
            RefreshCachedRouletteDictionary();
            RefreshLockedUpgrades();
        }
        private void RefreshCachedRouletteDictionary()
        {
            var listData = new List<UpgradeDataStruct>();
            var listWeigth = new List<float>();

            var dictionary = m_allAllUpgradeData.UpgradeDataStructs;
            for (int i = 0; i < m_unlockedUpgradeIDs.Count; i++)
            {
                if(!dictionary.TryGetValue(m_unlockedUpgradeIDs[i], out var upgradeDataStruct))
                    continue;
                
                listData.Add(upgradeDataStruct);
                listWeigth.Add(upgradeDataStruct.weight);
            }
            
            m_unlockedUpgradesRouletteWheel = new RouletteWheel<UpgradeDataStruct>(listData, listWeigth);
        }

        private void RefreshLockedUpgrades()
        {
            var allKeys = m_allAllUpgradeData.UpgradeDataStructs.Keys;
            var lockedUpgrades= ListExtensions.GetUnmatchedElements(allKeys.ToList(), m_unlockedUpgradeIDs);
            
            var listData = new List<UpgradeDataStruct>();
            var listWeigth = new List<float>();

            var dictionary = m_allAllUpgradeData.UpgradeDataStructs;
            for (int i = 0; i < m_lockedUpgradeIDs.Count; i++)
            {
                if(!dictionary.TryGetValue(m_lockedUpgradeIDs[i], out var upgradeDataStruct))
                    continue;
                
                listData.Add(upgradeDataStruct);
                listWeigth.Add(upgradeDataStruct.weight);
            }

            m_lockedUpgradeIDs = lockedUpgrades;
            m_lockedUpgradesRouletteWheel = new RouletteWheel<UpgradeDataStruct>(listData, listWeigth);
        }
        
        public List<UpgradeData> GetRandomUpgradesAndUnlock(int upgradesAmount)
        {
            List<UpgradeData> list = new List<UpgradeData>();

            for (int i = 0; i < upgradesAmount; i++)
            {
                var x = m_lockedUpgradesRouletteWheel.RunWithCached();
                list.Add(x.data);
                UnlockUpgrades(x);

            }

            return list;
        }

        public UpgradeData GetRandomUnlockedUpgradeFromPool() => m_unlockedUpgradesRouletteWheel.RunWithCached().data;

        public UpgradeData GetRandomLockedUpgradeFromPool() => m_lockedUpgradesRouletteWheel.RunWithCached().data;

        

        public void UnlockUpgrades(UpgradeDataStruct upgradeDataToUnlock)
        {
            m_unlockedUpgradeIDs.Add(upgradeDataToUnlock.id);
            RefreshUpgradeLists();
        }

        public void UnlockUpgrades(UpgradeData upgradeDataToUnlock)
        {
            var structData = m_allAllUpgradeData.GetStructByData(upgradeDataToUnlock);
            m_unlockedUpgradeIDs.Add(structData.id);
            RefreshUpgradeLists();
        }



        public UpgradeData GetUpgradeDataByID(string id)
        {
            foreach (var upgradeDataStruct in m_allAllUpgradeData.UpgradeDataStructs)
            {
                var element = upgradeDataStruct.Value;
                if (element.id.Equals(id))
                {
                    return element.data;
                }
            }

            Debug.LogError("Error in search for UpgradeData via id");
            return default;
        }
    }
}
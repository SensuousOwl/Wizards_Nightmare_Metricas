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
            RefreshUnlockedUpgradesDictionary();
            RefreshLockedUpgradesDictionary();
        }
        private void RefreshUnlockedUpgradesDictionary()
        {
            var l_listData = new List<UpgradeDataStruct>();
            var l_listWeigth = new List<float>();

            var l_dictionary = m_allAllUpgradeData.UpgradeDataStructs;
            
            for (int l_i = 0; l_i < m_unlockedUpgradeIDs.Count; l_i++)
            {
                if(!l_dictionary.TryGetValue(m_unlockedUpgradeIDs[l_i], out var l_upgradeDataStruct))
                    continue;
                
                l_listData.Add(l_upgradeDataStruct);
                l_listWeigth.Add(l_upgradeDataStruct.weight);
            }
            
            m_unlockedUpgradesRouletteWheel = new RouletteWheel<UpgradeDataStruct>(l_listData, l_listWeigth);
        }

        private void RefreshLockedUpgradesDictionary()
        {
            var l_allKeys = m_allAllUpgradeData.UpgradeDataStructs.Keys;
            m_lockedUpgradeIDs = ListExtensions.GetUnmatchedElements(l_allKeys.ToList(), m_unlockedUpgradeIDs);
            
            var l_listData = new List<UpgradeDataStruct>();
            var l_listWeigth = new List<float>();

            var l_dictionary = m_allAllUpgradeData.UpgradeDataStructs;
            
            
            for (int l_i = 0; l_i < m_lockedUpgradeIDs.Count; l_i++)
            {
                if(!l_dictionary.TryGetValue(m_lockedUpgradeIDs[l_i], out var l_upgradeDataStruct))
                    continue;
                
                l_listData.Add(l_upgradeDataStruct);
                l_listWeigth.Add(l_upgradeDataStruct.weight);
            }

            m_lockedUpgradesRouletteWheel = new RouletteWheel<UpgradeDataStruct>(l_listData, l_listWeigth);
        }


        
        public List<UpgradeData> GetRandomUpgradesAndUnlock(int p_upgradesAmount)
        {
            List<UpgradeData> l_list = new List<UpgradeData>();

            for (int l_i = 0; l_i < p_upgradesAmount; l_i++)
            {
                var l_x = m_lockedUpgradesRouletteWheel.RunWithCached();
                l_list.Add(l_x.data);
                UnlockUpgrades(l_x);

            }

            return l_list;
        }

        public UpgradeData GetRandomUnlockedUpgradeFromPool()
        {
            if (m_unlockedUpgradesRouletteWheel.IsEmpty())
                return default;
            
            return m_unlockedUpgradesRouletteWheel.RunWithCached().data;
        }
        
        public UpgradeData GetRandomUnlockedUpgradeFromPool(List<UpgradeData> p_upgradesExclude)
        {
            if (m_unlockedUpgradesRouletteWheel.IsEmpty())
                return default;
            
            var l_upgrade = m_unlockedUpgradesRouletteWheel.RunWithCached().data;
            var l_watchDog = 1000;

            while (p_upgradesExclude.Contains(l_upgrade) && l_watchDog > 0)
            {
                l_upgrade = m_unlockedUpgradesRouletteWheel.RunWithCached().data;
                l_watchDog--;
            }

            return l_upgrade;
        }

        public UpgradeData GetRandomLockedUpgradeFromPool() => m_lockedUpgradesRouletteWheel.RunWithCached().data;

        

        public void UnlockUpgrades(UpgradeDataStruct p_upgradeDataToUnlock)
        {
            m_unlockedUpgradeIDs.Add(p_upgradeDataToUnlock.id);
            RefreshUpgradeLists();
        }

        public void UnlockUpgrades(UpgradeData p_upgradeDataToUnlock)
        {
            var l_structData = m_allAllUpgradeData.GetStructByData(p_upgradeDataToUnlock);
            m_unlockedUpgradeIDs.Add(l_structData.id);
            RefreshUpgradeLists();
        }



        public UpgradeData GetUpgradeDataByID(string p_id)
        {
            foreach (var l_upgradeDataStruct in m_allAllUpgradeData.UpgradeDataStructs)
            {
                var l_element = l_upgradeDataStruct.Value;
                if (l_element.id.Equals(p_id))
                {
                    return l_element.data;
                }
            }

            Debug.LogError("Error in search for UpgradeData via id");
            return default;
        }
    }
}
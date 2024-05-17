using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using _Main.Scripts.Services.MicroServices.UserDataService;
using _Main.Scripts.Services.MicroServices.UserDataService.UserDataStates;
using _Main.Scripts.StaticClass;

namespace _Main.Scripts.Services.UpgradePoolServices
{
    public class UpgradePoolService : IUpgradePoolService
    {
        private AllUpgradeDataPool m_allAllUpgradeData;
        
        
        private RouletteWheel<UpgradeData> m_unlockedUpgradesRouletteWheel;
        private RouletteWheel<UpgradeData> m_lockedUpgradesRouletteWheel;

        private UpgradesServiceDataState DataState => ServiceLocator.Get<IUserDataService>().GetState<UpgradesServiceDataState>();
        
        
        public void Initialize()
        {
            m_allAllUpgradeData = MyGame.AllUpgradePoolDataEssentials;

            if (DataState.GetUnlockedUpgrades() == default)
                DataState.SetUnlockedUpgrades(m_allAllUpgradeData.DefaultUnlockedUpgrades);
            
            RefreshUpgradeLists();
        }
        
        
        
        private void RefreshUpgradeLists()
        {
            RefreshUnlockedUpgradesDictionary();
            RefreshLockedUpgradesDictionary();
        }
        private void RefreshUnlockedUpgradesDictionary()
        {
            var l_dataDictionary = new Dictionary<UpgradeData, float>();

            var l_dictionary = m_allAllUpgradeData.UpgradesDataDictionary;

            var l_unlocked = DataState.GetUnlockedUpgrades();
            
            for (int l_i = 0; l_i < l_unlocked.Count; l_i++)
            {
                if(!l_dictionary.TryGetValue(l_unlocked[l_i], out var l_upgradeDataStruct))
                    continue;
                l_dataDictionary.TryAdd(l_upgradeDataStruct, l_upgradeDataStruct.UpgradeWeight);
            }
            
            m_unlockedUpgradesRouletteWheel = new RouletteWheel<UpgradeData>(l_dataDictionary);
        }

        private void RefreshLockedUpgradesDictionary()
        {
            var l_dataDictionary = new Dictionary<UpgradeData, float>();
            
            var l_allKeys = m_allAllUpgradeData.UpgradesDataDictionary.Keys;
            var l_lockedUpgradeIDs = ListExtensions.GetUnmatchedElements(l_allKeys, DataState.GetUnlockedUpgrades());
            
            var l_dictionary = m_allAllUpgradeData.UpgradesDataDictionary;
            
            for (int l_i = 0; l_i < l_lockedUpgradeIDs.Count; l_i++)
            {
                if(!l_dictionary.TryGetValue(l_lockedUpgradeIDs[l_i], out var l_upgradeDataStruct))
                    continue;
                
                l_dataDictionary.TryAdd(l_upgradeDataStruct, l_upgradeDataStruct.UnlockWeight);
            }

            
            m_lockedUpgradesRouletteWheel = new RouletteWheel<UpgradeData>(l_dataDictionary);
        }


        
        public List<UpgradeData> GetRandomUpgradesAndUnlock(int p_upgradesAmount)
        {
            List<UpgradeData> l_list = new List<UpgradeData>();

            for (int l_i = 0; l_i < p_upgradesAmount; l_i++)
            {
                var l_x = m_lockedUpgradesRouletteWheel.RunWithCached();
                l_list.Add(l_x);
                UnlockUpgrades(l_x.Id);

            }

            return l_list;
        }

        public UpgradeData GetRandomUnlockedUpgradeFromPool()
        {
            if (m_unlockedUpgradesRouletteWheel.IsEmpty())
                return default;
            
            return m_unlockedUpgradesRouletteWheel.RunWithCached();
        }
        
        public UpgradeData GetRandomUnlockedUpgradeFromPool(List<UpgradeData> p_upgradesExclude)
        {
            if (m_unlockedUpgradesRouletteWheel.IsEmpty())
                return default;
            
            var l_upgrade = m_unlockedUpgradesRouletteWheel.RunWithCached();
            var l_watchDog = 1000;

            while (p_upgradesExclude.Contains(l_upgrade) && l_watchDog > 0)
            {
                l_upgrade = m_unlockedUpgradesRouletteWheel.RunWithCached();
                l_watchDog--;
            }

            return l_upgrade;
        }

        public UpgradeData GetRandomLockedUpgradeFromPool() => m_lockedUpgradesRouletteWheel.RunWithCached();

        public void UnlockUpgrades(string p_upgradeId)
        {
            DataState.AddUnlockedUpgrade(p_upgradeId);
            RefreshUpgradeLists();
        }



        public UpgradeData GetUpgradeDataByID(string p_id)
        {
            if (m_allAllUpgradeData.UpgradesDataDictionary.TryGetValue(p_id, out var l_struct))
                return l_struct;
            
            Logger.LogError("Error in search for UpgradeData via id");
            return default;
        }
    }
}
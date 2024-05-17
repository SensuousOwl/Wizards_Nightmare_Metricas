using System.Collections.Generic;
using _Main.Scripts.StaticClass;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.UserDataService.UserDataStates
{
    public class UpgradesServiceDataState : IUserState
    {
        [JsonProperty] private List<string> m_unlockedUpgrades;


        public void ResetUnlockedUpgrades()
        {
            var l_baseUpgrades = MyGame.AllUpgradePoolDataEssentials.DefaultUnlockedUpgrades;
            m_unlockedUpgrades = l_baseUpgrades;
            CheckUnlockedUpgradesIds();
        }
        
        public List<string> GetUnlockedUpgrades() => m_unlockedUpgrades;
        public void SetUnlockedUpgrades(List<string> p_list)
        {
            m_unlockedUpgrades = p_list;
            CheckUnlockedUpgradesIds();
        }
        public void AddUnlockedUpgrade(string x)
        {
            m_unlockedUpgrades.Add(x);
            CheckUnlockedUpgradesIds();
        }

        public void CheckUnlockedUpgradesIds()
        {
            var l_allUpgrades = MyGame.AllUpgradePoolDataEssentials.UpgradesDataDictionary;
            for (int i = 0; i < m_unlockedUpgrades.Count; i++)
            {
                var l_currId = m_unlockedUpgrades[i];
                if(l_allUpgrades.ContainsKey(l_currId))
                    continue;
                
                m_unlockedUpgrades.Remove(l_currId);
                Logger.LogWarning($"Removed a id in unlocked json, id: {l_currId}");
            }
        }
    }
}
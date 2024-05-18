using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.UserDataService.UserDataStates
{
    public class UpgradesServiceDataState : IUserState
    {
        [JsonProperty] private List<string> unlockedUpgrades;


        public void ResetUnlockedUpgrades()
        {
            var l_baseUpgrades = MyGame.AllUpgradePoolDataEssentials.DefaultUnlockedUpgrades;
            unlockedUpgrades = l_baseUpgrades;
            CheckUnlockedUpgradesIds();
        }
        
        public List<string> GetUnlockedUpgrades() => unlockedUpgrades;
        public void SetUnlockedUpgrades(List<string> p_list)
        {
            unlockedUpgrades = p_list;
            CheckUnlockedUpgradesIds();
        }
        public void AddUnlockedUpgrade(string x)
        {
            unlockedUpgrades.Add(x);
            CheckUnlockedUpgradesIds();
        }

        public void CheckUnlockedUpgradesIds()
        {
            var l_allUpgrades = MyGame.AllUpgradePoolDataEssentials.UpgradesDataDictionary;
            for (int i = 0; i < unlockedUpgrades.Count; i++)
            {
                var l_currId = unlockedUpgrades[i];
                if(l_allUpgrades.ContainsKey(l_currId))
                    continue;
                
                unlockedUpgrades.Remove(l_currId);
                Logger.LogWarning($"Removed a id in unlocked json, id: {l_currId}");
            }
        }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.UserDataService.UserDataStates
{
    public class UpgradesServiceDataState : IUserState
    {
        [JsonProperty] private List<string> unlockedUpgrades;

        
        
        public List<string> GetUnlockedUpgrades() => unlockedUpgrades;
        public void SetUnlockedUpgrades(List<string> p_list) => unlockedUpgrades = p_list;
        public void AddUnlockedUpgrade(string x) => unlockedUpgrades.Add(x);
    }
}
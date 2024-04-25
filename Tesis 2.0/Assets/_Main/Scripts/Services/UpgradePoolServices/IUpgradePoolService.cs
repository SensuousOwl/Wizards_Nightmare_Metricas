using System.Collections.Generic;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;

namespace _Main.Scripts.Services.UpgradePoolServices
{
    public interface IUpgradePoolService : IGameService
    {
        public List<UpgradeData> GetRandomUpgradesAndUnlock(int upgradesAmount);
        public UpgradeData GetRandomUnlockedUpgradeFromPool();
        public UpgradeData GetRandomUnlockedUpgradeFromPool(List<UpgradeData> p_upgradesExclude);
        public UpgradeData GetRandomLockedUpgradeFromPool();
        
        public void UnlockUpgrades(UpgradeDataStruct upgradeDataToUnlock);
        public void UnlockUpgrades(UpgradeData upgradeDataToUnlock);
    }
}
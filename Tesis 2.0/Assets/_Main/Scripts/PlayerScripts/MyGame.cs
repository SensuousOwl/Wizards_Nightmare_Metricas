using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using UnityEngine;

namespace _Main.Scripts.PlayerScripts
{
    public static class MyGame
    {
        
        private const string PLAYER_DATA_ESSENTIALS_FILE_NAME = "PlayerDataEssentials";
        private const string ALL_UPGRADES_POOL_DATA_ESSENTIALS_FILE_NAME = "AllUpgradeDataEssentials";
        private const string ITEMS_POOL_DATA_ESSENTIALS_FILE_NAME = "ItemsDataEssentials";
       

        private static PlayerData m_playerDataEssentials;
        public static PlayerData PlayerDataEssentials => GetGameResource(ref m_playerDataEssentials, PLAYER_DATA_ESSENTIALS_FILE_NAME);
        
        
        private static AllUpgradeDataPool m_allUpgradeDataPoolEssentials;
        public static AllUpgradeDataPool AllUpgradePoolDataEssentials => GetGameResource(ref m_allUpgradeDataPoolEssentials, ALL_UPGRADES_POOL_DATA_ESSENTIALS_FILE_NAME);
        
        
        private static ItemsDataPoolEssentials m_itemsDataPoolEssentials;
        public static ItemsDataPoolEssentials ItemsDataPoolEssentials => GetGameResource(ref m_itemsDataPoolEssentials, ITEMS_POOL_DATA_ESSENTIALS_FILE_NAME);
        
        
        private static T GetGameResource<T>(ref T p_localVariable, string p_filePath) where T : ScriptableObject
        {
            if (p_localVariable != null)
                return p_localVariable;
            if (p_localVariable == null)
                p_localVariable = (T)Resources.Load(p_filePath, typeof(T));
            if (p_localVariable == null)
                Debug.LogError($"Asset '{p_filePath}' not found.");
            if (p_localVariable is IInitializable l_initializable)
            {
                l_initializable.Init();
            }          
            
            return p_localVariable;
        }
    }
}
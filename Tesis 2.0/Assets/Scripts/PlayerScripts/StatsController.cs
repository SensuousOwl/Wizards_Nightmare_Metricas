using System.Collections.Generic;

namespace PlayerScripts
{
    public enum StatsId
    {
        MovementSpeed,
        Energy,
        FireRate,
        Range,
        CriticalChance,
        DashCooldown,
        DashTranslation,
        ProjectileSpeed,
        Damage
    }
    
    public class StatsController
    {
        private readonly Dictionary<StatsId, float> m_statsDictionary = new();

        public StatsController(PlayerData p_playerData)
        {
            m_statsDictionary.Add(StatsId.MovementSpeed, p_playerData.MovementSpeed);
            m_statsDictionary.Add(StatsId.Energy, p_playerData.Energy);
            m_statsDictionary.Add(StatsId.FireRate, p_playerData.FireRate);
            m_statsDictionary.Add(StatsId.Range, p_playerData.Range);
            m_statsDictionary.Add(StatsId.CriticalChance, p_playerData.CriticalChance);
            m_statsDictionary.Add(StatsId.DashCooldown, p_playerData.DashCooldown);
            m_statsDictionary.Add(StatsId.DashTranslation, p_playerData.DashTranslation);
            m_statsDictionary.Add(StatsId.ProjectileSpeed, p_playerData.ProjectileSpeed);
            m_statsDictionary.Add(StatsId.Damage, p_playerData.Damage);
        }

        public float GetStatById(StatsId p_statsId)
        {
            return m_statsDictionary[p_statsId];
        }

        public bool TryGetStatById(StatsId p_statsId, out float p_value) =>
            m_statsDictionary.TryGetValue(p_statsId, out p_value);

        public void SetUpgradeStat(StatsId p_statsId, float p_newValue)
        {
            if (!m_statsDictionary.ContainsKey(p_statsId))
                return;
            m_statsDictionary[p_statsId] = p_newValue;
        }

        public void AddUpgradeStat(StatsId p_statsId, float p_addValue)
        {
            if (!m_statsDictionary.ContainsKey(p_statsId))
                return;
            
            m_statsDictionary[p_statsId] += p_addValue;
        }
        
        public void AddUpgradeStatForPercentage(StatsId p_statsId, float p_percentage)
        {
            if (!m_statsDictionary.TryGetValue(p_statsId, out var l_value))
                return;

            l_value *= p_percentage / 100;
            
            m_statsDictionary[p_statsId] += l_value;
        }
    }
}
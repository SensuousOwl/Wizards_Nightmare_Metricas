using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;

namespace _Main.Scripts.Services.Stats
{
    public class StatsService : IStatsService
    {
        public void Initialize()
        {
            var data = MyGame.PlayerDataEssentials;
            
            m_statsDictionary.Add(StatsId.MovementSpeed, data.MovementSpeed);
            m_statsDictionary.Add(StatsId.CriticalDamageMult, data.CriticalDamageMult);
            m_statsDictionary.Add(StatsId.SpawnItemChance, data.SpawnItemChance);
            m_statsDictionary.Add(StatsId.FireRate, data.FireRate);
            m_statsDictionary.Add(StatsId.Range, data.FireRange);
            m_statsDictionary.Add(StatsId.CriticalChance, data.CriticalChance);
            m_statsDictionary.Add(StatsId.DashCooldown, data.DashCooldown);
            m_statsDictionary.Add(StatsId.DashTranslation, data.DashTranslation);
            m_statsDictionary.Add(StatsId.ProjectileSpeed, data.ProjectileSpeed);
            m_statsDictionary.Add(StatsId.Damage, data.Damage);
        }
        
        
        private readonly Dictionary<StatsId, float> m_statsDictionary = new();

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
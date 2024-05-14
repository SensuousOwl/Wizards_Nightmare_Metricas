using System;
using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;

namespace _Main.Scripts.Services.Stats
{
    public class StatsService : IStatsService
    {
        public void Initialize()
        {
            var l_data = MyGame.PlayerDataEssentials;
            
            m_statsDictionary.Add(StatsId.MovementSpeed, l_data.MovementSpeed);
            m_statsDictionary.Add(StatsId.CriticalDamageMult, l_data.CriticalDamageMult);
            m_statsDictionary.Add(StatsId.SpawnItemChance, l_data.SpawnItemChance);
            m_statsDictionary.Add(StatsId.FireRate, l_data.FireRate);
            m_statsDictionary.Add(StatsId.Range, l_data.FireRange);
            m_statsDictionary.Add(StatsId.CriticalChance, l_data.CriticalChance);
            m_statsDictionary.Add(StatsId.DashCooldown, l_data.DashCooldown);
            m_statsDictionary.Add(StatsId.DashTranslation, l_data.DashTranslation);
            m_statsDictionary.Add(StatsId.ProjectileSpeed, l_data.ProjectileSpeed);
            m_statsDictionary.Add(StatsId.Damage, l_data.Damage);
            m_statsDictionary.Add(StatsId.MaxHealth, l_data.MaxHp);
            m_statsDictionary.Add(StatsId.SubtractItemActiveCooldown, l_data.SubtractItemActiveCooldownPercentage);
        }
        
        
        private readonly Dictionary<StatsId, float> m_statsDictionary = new();


        public Action<StatsId, float> OnChangeStatValue { get; set; }

        public Dictionary<StatsId, float> GetAllStatData() => m_statsDictionary;

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
            OnChangeStatValue.Invoke(p_statsId, m_statsDictionary[p_statsId]);
        }

        public void AddUpgradeStat(StatsId p_statsId, float p_addValue)
        {
            if (!m_statsDictionary.ContainsKey(p_statsId))
                return;
            
            m_statsDictionary[p_statsId] += p_addValue;
            OnChangeStatValue.Invoke(p_statsId, m_statsDictionary[p_statsId]);
        }

        public void SubtractUpgradeStat(StatsId p_statsId, float p_subtractValue)
        {
            if (!m_statsDictionary.ContainsKey(p_statsId))
                return;
            
            m_statsDictionary[p_statsId] -= p_subtractValue;
            OnChangeStatValue.Invoke(p_statsId, m_statsDictionary[p_statsId]);
        }

        public void AddUpgradeStatForPercentage(StatsId p_statsId, float p_percentage)
        {
            if (!m_statsDictionary.TryGetValue(p_statsId, out var l_value))
                return;

            l_value *= p_percentage / 100;
            
            m_statsDictionary[p_statsId] += l_value;
            OnChangeStatValue.Invoke(p_statsId, m_statsDictionary[p_statsId]);
        }
    }
}
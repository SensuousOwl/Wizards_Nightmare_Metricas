using System;
using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;

namespace _Main.Scripts.Services.Stats
{
    public class StatsService : IStatsService
    {
        private Dictionary<StatsId, float> m_baseStatsDictionary = new();
        private Dictionary<StatsId, float> m_maxStatsValueDictionary = new();
        private Dictionary<StatsId, float> m_currentStatsDictionary = new();

        public event Action<StatsId, float> OnChangeStatValue;
        
        public void Initialize()
        {
            var l_data = MyGame.PlayerDataEssentials;

            m_baseStatsDictionary = l_data.BaseStatsDictionary.GetNewDictionary();
            m_maxStatsValueDictionary = l_data.MaxStatsValueDictionary.GetNewDictionary();
            m_currentStatsDictionary = new Dictionary<StatsId, float>(m_baseStatsDictionary);
        }

        public Dictionary<StatsId, float> GetAllStatData() => m_currentStatsDictionary;

        public float GetStatById(StatsId p_statsId)
        {
            return m_currentStatsDictionary[p_statsId];
        }
        
        public float GetBaseStatById(StatsId p_statsId)
        {
            return m_baseStatsDictionary[p_statsId];
        }

        public bool TryGetStatById(StatsId p_statsId, out float p_value) =>
            m_currentStatsDictionary.TryGetValue(p_statsId, out p_value);

        public void SetUpgradeStat(StatsId p_statsId, float p_newValue)
        {
            if (!m_currentStatsDictionary.ContainsKey(p_statsId))
                return;
            m_currentStatsDictionary[p_statsId] = p_newValue;
            
            CheckUpgradeIsInBounds(p_statsId);
            
            OnChangeStatValue?.Invoke(p_statsId, m_currentStatsDictionary[p_statsId]);
        }

        public void AddUpgradeStat(StatsId p_statsId, float p_addValue)
        {
            if (!m_currentStatsDictionary.ContainsKey(p_statsId))
                return;
            
            m_currentStatsDictionary[p_statsId] += p_addValue;
            
            CheckUpgradeIsInBounds(p_statsId);
            
            OnChangeStatValue?.Invoke(p_statsId, m_currentStatsDictionary[p_statsId]);
        }

        public void SubtractUpgradeStat(StatsId p_statsId, float p_subtractValue)
        {
            if (!m_currentStatsDictionary.ContainsKey(p_statsId))
                return;
            
            m_currentStatsDictionary[p_statsId] -= p_subtractValue;
            
            CheckUpgradeIsInBounds(p_statsId);
            
            OnChangeStatValue?.Invoke(p_statsId, m_currentStatsDictionary[p_statsId]);
        }

        public void AddUpgradeStatForPercentage(StatsId p_statsId, float p_percentage)
        {
            if (!m_currentStatsDictionary.TryGetValue(p_statsId, out var l_value))
                return;

            l_value *= p_percentage / 100;
            
            m_currentStatsDictionary[p_statsId] += l_value;
            
            CheckUpgradeIsInBounds(p_statsId);
            OnChangeStatValue?.Invoke(p_statsId, m_currentStatsDictionary[p_statsId]);
        }

        public void SubtractUpgradeStatForPercentage(StatsId p_statsId, float p_percentage)
        {
            if (!m_currentStatsDictionary.TryGetValue(p_statsId, out var l_value))
                return;

            l_value *= p_percentage / 100;
            
            m_currentStatsDictionary[p_statsId] -= l_value;

            CheckUpgradeIsInBounds(p_statsId);
            
            OnChangeStatValue?.Invoke(p_statsId, m_currentStatsDictionary[p_statsId]);
        }


        private void CheckUpgradeIsInBounds(StatsId p_statsId)
        {
            if (m_currentStatsDictionary[p_statsId] < m_baseStatsDictionary[p_statsId])
                m_currentStatsDictionary[p_statsId] = m_baseStatsDictionary[p_statsId];
            
            if (m_currentStatsDictionary[p_statsId] > m_maxStatsValueDictionary[p_statsId])
                m_currentStatsDictionary[p_statsId] = m_maxStatsValueDictionary[p_statsId];
        }
    }
}
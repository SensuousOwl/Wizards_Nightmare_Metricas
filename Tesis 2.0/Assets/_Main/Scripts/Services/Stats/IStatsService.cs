using System;
using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;

namespace _Main.Scripts.Services.Stats
{
    public interface IStatsService : IGameService
    {
        public event Action<StatsId, float> OnChangeStatValue;
        public Dictionary<StatsId, float> GetAllStatData();

        public float GetStatById(StatsId p_statsId);
        public float GetBaseStatById(StatsId p_statsId);

        public bool TryGetStatById(StatsId p_statsId, out float p_value);

        public void SetUpgradeStat(StatsId p_statsId, float p_newValue);

        public void AddUpgradeStat(StatsId p_statsId, float p_addValue);
        public void SubtractUpgradeStat(StatsId p_statsId, float p_addValue);

        public void AddUpgradeStatForPercentage(StatsId p_statsId, float p_percentage);
        public void SubtractUpgradeStatForPercentage(StatsId p_statsId, float p_percentage);
    }
}
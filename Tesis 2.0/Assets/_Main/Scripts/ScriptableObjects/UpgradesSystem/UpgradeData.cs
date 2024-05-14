using System;
using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{

    [Serializable]
    public struct UpgradeConfig
    {
        [field: SerializeField] public StatsId StatId { get; private set; }
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        [field: SerializeField] public bool Subtract { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
    
    [Serializable]
    public enum UpgradeType
    {
        Simple,
        Percentage
    }
    
    [CreateAssetMenu(menuName = "Main/Upgrades/UpgradeData")]
    public class UpgradeData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }
        [field: SerializeField] public float UpgradeWeight { get; private set; }
        [field: SerializeField] public float UnlockWeight { get; private set; }
        [SerializeField] private List<UpgradeConfig> upgradeConfigs;
        [field: SerializeField] public Sprite BorderSprite { get; private set; }
        [field: SerializeField] public Sprite EffectSprite { get; private set; }

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();

        public void ApplyEffects()
        { 
            Logger.Log("Apply upgrade: " + name);

            foreach (var l_upgradeConfig in upgradeConfigs)
            {
                switch (l_upgradeConfig.UpgradeType)
                {
                    case UpgradeType.Simple:
                        if (!l_upgradeConfig.Subtract)
                            StatsService.AddUpgradeStat(l_upgradeConfig.StatId, l_upgradeConfig.Value);
                        else
                            StatsService.SubtractUpgradeStat(l_upgradeConfig.StatId, l_upgradeConfig.Value);
                        continue;
                    case UpgradeType.Percentage:
                        if (!l_upgradeConfig.Subtract)
                            StatsService.AddUpgradeStatForPercentage(l_upgradeConfig.StatId, l_upgradeConfig.Value);
                        else
                            StatsService.SubtractUpgradeStatForPercentage(l_upgradeConfig.StatId, l_upgradeConfig.Value);
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
#if UNITY_EDITOR
        
        [ContextMenu("SetRandomID")]
        private void SetRandomID()
        {
            Id = "RandomID" + Random.Range(0, 10000);
        }


        
        #endif
    }
}
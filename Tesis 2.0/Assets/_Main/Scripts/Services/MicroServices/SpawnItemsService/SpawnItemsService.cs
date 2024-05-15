using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Enemies;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.SpawnItemsService
{
    public class SpawnItemsService : ISpawnItemsService
    {
        private static ItemsDataPoolEssentials Data => MyGame.ItemsDataPoolEssentials;
        private RouletteWheel<RouletteWheel<ItemData>> m_genericRouletteWheel;
        private RouletteWheel<RouletteWheel<ItemData>> m_chestRouletteWheel;
        private RouletteWheel<RouletteWheel<ItemData>> m_bossRouletteWheel;

        private static IEventService m_eventService;
        private static IStatsService m_statsService;

        public SpawnItemsService(IEventService p_eventService, IStatsService p_statsService)
        {
            m_eventService = p_eventService;
            m_statsService = p_statsService;
        }
        
        public void Initialize()
        {
            var l_commonRouletteWheel = new RouletteWheel<ItemData>(Data.CommonItems.GetDictionary());
            var l_rareRouletteWheel = new RouletteWheel<ItemData>(Data.RareItems.GetDictionary());
            var l_epicRouletteWheel = new RouletteWheel<ItemData>(Data.EpicItems.GetDictionary());

            var l_auxGenericDictionary = new Dictionary<RouletteWheel<ItemData>, float>
            {
                { l_commonRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Generic].CommonItemsWeight },
                { l_rareRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Generic].RareItemsWeight },
                { l_epicRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Generic].EpicItemsWeight },
            };
            m_genericRouletteWheel = new RouletteWheel<RouletteWheel<ItemData>>(l_auxGenericDictionary);
            
            var l_auxChestDictionary = new Dictionary<RouletteWheel<ItemData>, float>
            {
                { l_commonRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Chest].CommonItemsWeight },
                { l_rareRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Chest].RareItemsWeight },
                { l_epicRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Chest].EpicItemsWeight },
            };
            m_chestRouletteWheel = new RouletteWheel<RouletteWheel<ItemData>>(l_auxChestDictionary);
            
            var l_auxBossDictionary = new Dictionary<RouletteWheel<ItemData>, float>
            {
                { l_commonRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Boss].CommonItemsWeight },
                { l_rareRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Boss].RareItemsWeight },
                { l_epicRouletteWheel, Data.ItemsPoolWeight[TypeDropItemPool.Boss].EpicItemsWeight },
            };
            m_bossRouletteWheel = new RouletteWheel<RouletteWheel<ItemData>>(l_auxBossDictionary);
            
            m_eventService?.AddListener<DieEnemyEventData>(SpawnItemHandler);
        }

        ~SpawnItemsService()
        {
            m_eventService?.RemoveListener<DieEnemyEventData>(SpawnItemHandler);
        }

        private void SpawnItemHandler(DieEnemyEventData p_data)
        {
            if (p_data.Model.GetData().IsBoss)
            {
                SpawnItem(m_bossRouletteWheel.RunWithCached().RunWithCached(), p_data.PositionNode);
                return;
            }
            
            var l_value = Random.Range(0f, 100f);
            if (l_value <= m_statsService.GetStatById(StatsId.SpawnItemChance))
                SpawnRandomItem(p_data.PositionNode);
        }


        public void SpawnItem(ItemData p_itemToSpawn, Vector3 p_positionToSpawn)
        {
            var l_newItem = Object.Instantiate(Data.ItemPrefab, p_positionToSpawn, Quaternion.identity);
            l_newItem.SetItemData(p_itemToSpawn);
        }
        public void SpawnItemChestPoolRandom(Vector3 p_positionToSpawn)
        {
            SpawnItem(m_chestRouletteWheel.RunWithCached().RunWithCached(), p_positionToSpawn);
        }
        
        public void SpawnRandomItem(Vector3 p_positionToSpawn)
        {
            var l_itemData = m_genericRouletteWheel.RunWithCached().RunWithCached();

            SpawnItem(l_itemData, p_positionToSpawn);
        }
    }

    public struct DieEnemyEventData : ICustomEventData
    {
        public EnemyModel Model { get; }
        public Vector3 PositionNode { get; }
        
        public DieEnemyEventData(Vector3 p_positionNode, EnemyModel p_model)
        {
            PositionNode = p_positionNode;
            Model = p_model;
        }
    }
}
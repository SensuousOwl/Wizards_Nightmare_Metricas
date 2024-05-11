using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.SpawnItemsService
{
    public interface ISpawnItemsService : IGameService
    {
        public void SpawnItem(ItemData p_itemToSpawn, Vector3 p_positionToSpawn);
        public void SpawnRandomItem(Vector3 p_positionToSpawn);
    }
    
    public class SpawnItemsService : ISpawnItemsService
    {
        private static ItemsDataPoolEssentials Data => MyGame.ItemsDataPoolEssentials;
        private RouletteWheel<RouletteWheel<ItemData>> m_rouletteWheel;

        private static IEventService m_eventService;

        public SpawnItemsService(IEventService p_eventService)
        {
            m_eventService = p_eventService;
        }
        
        public void Initialize()
        {
            var l_commonRouletteWheel = new RouletteWheel<ItemData>(Data.CommonItems.GetDictionary());
            var l_rareRouletteWheel = new RouletteWheel<ItemData>(Data.RareItems.GetDictionary());
            var l_epicRouletteWheel = new RouletteWheel<ItemData>(Data.EpicItems.GetDictionary());

            var l_auxDictionary = new Dictionary<RouletteWheel<ItemData>, float>
            {
                { l_commonRouletteWheel, Data.CommonItemsWeight },
                { l_rareRouletteWheel, Data.RareItemsWeight },
                { l_epicRouletteWheel, Data.EpicItemsWeight }
            };

            m_rouletteWheel = new RouletteWheel<RouletteWheel<ItemData>>(l_auxDictionary);
            
            m_eventService?.AddListener<SpawnItemEventData>(SpawnItemHandler);
        }

        ~SpawnItemsService()
        {
            m_eventService?.RemoveListener<SpawnItemEventData>(SpawnItemHandler);
        }

        private void SpawnItemHandler(SpawnItemEventData p_data)
        {
            SpawnRandomItem(p_data.PositionToSpawn);
        }


        public void SpawnItem(ItemData p_itemToSpawn, Vector3 p_positionToSpawn)
        {
            var l_newItem = Object.Instantiate(Data.ItemPrefab, p_positionToSpawn, Quaternion.identity);
            l_newItem.SetItemData(p_itemToSpawn);
        }
        
        public void SpawnRandomItem(Vector3 p_positionToSpawn)
        {
            var l_itemData = m_rouletteWheel.RunWithCached().RunWithCached();

            SpawnItem(l_itemData, p_positionToSpawn);
        }
    }

    public struct SpawnItemEventData : ICustomEventData
    {
        public Vector3 PositionToSpawn { get; }
        
        public SpawnItemEventData(Vector3 p_positionToSpawn)
        {
            PositionToSpawn = p_positionToSpawn;
        }
    }
}
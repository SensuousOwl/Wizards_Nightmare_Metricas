using _Main.Scripts.ScriptableObjects.ItemsSystem;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.SpawnItemsService
{
    public interface ISpawnItemsService : IGameService
    {
        public void SpawnItem(ItemData p_itemToSpawn, Vector3 p_positionToSpawn);
        public void SpawnRandomItem(Vector3 p_positionToSpawn);
    }
}
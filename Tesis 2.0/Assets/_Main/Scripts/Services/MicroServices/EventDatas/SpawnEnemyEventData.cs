using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct SpawnEnemyEventData : ICustomEventData
    {
        public Room Room { get; }
        public EnemyModel EnemyModelPrefab{ get; }
        public Vector3 SpawnPoint{ get; }

        public SpawnEnemyEventData(Room p_room, EnemyModel p_enemyModelPrefab, Vector3 p_spawnPoint)
        {
            Room = p_room;
            EnemyModelPrefab = p_enemyModelPrefab;
            SpawnPoint = p_spawnPoint;
        }
    }
}
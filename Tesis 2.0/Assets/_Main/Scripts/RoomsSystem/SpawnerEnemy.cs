using System.Collections.Generic;
using _Main.Scripts.ScriptableObjects;
using Enemies;
using Services;
using Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.RoomsSystem
{
    public class SpawnerEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyPoolData enemyPoolData;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        private int m_enemyCount;
        private Room m_currentRoom;
        private void OnEnable()
        {
            EventService.AddListener<SpawnEnemyEventData>(Callback);
        }

        private void Callback(SpawnEnemyEventData p_data)
        {
            m_currentRoom = p_data.Room;
            m_enemyCount = 0;
            var l_countSpawn = Random.Range(m_currentRoom.MinEnemySpawn, m_currentRoom.MaxEnemySpawn + 1);
            for (var l_i = 0; l_i < l_countSpawn; l_i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[Random.Range(0, m_currentRoom.SpawnPoints.Count)];
                var l_enemyPrefab = enemyPoolData.GetRandomEnemyPrefabFromPool();
                var l_enemyModel = Instantiate(l_enemyPrefab, l_spawnPoint.position, l_enemyPrefab.transform.rotation);
                l_enemyModel.OnDie += DieEnemyHandler;
                m_enemyCount++;
            }
        }

        private void DieEnemyHandler(EnemyModel p_enemyModel)
        {
            p_enemyModel.OnDie -= DieEnemyHandler;
            m_enemyCount--;

            if (m_enemyCount <= 0)
                m_currentRoom.ClearRoom();

        }
    }

    public struct SpawnEnemyEventData : ICustomEventData
    {
        public Room Room { get; }

        public SpawnEnemyEventData(Room p_room)
        {
            Room = p_room;
        }
    }
}
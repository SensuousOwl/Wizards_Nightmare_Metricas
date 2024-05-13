using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Enemies;
using _Main.Scripts.ScriptableObjects;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class SpawnerEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyPoolData enemyPoolData;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        private Room m_currentRoom;
        private readonly List<EnemyModel> m_enemies = new();
        
        private void OnEnable()
        {
            EventService.AddListener<SpawnEnemiesInRoomEventData>(SpawnEnemiesInRoomHandler);
            EventService.AddListener<SpawnEnemyEventData>(SpawnEnemyHandler);
            EventService.AddListener<SpawnBossInRoom>(OnSpawnBossInRoomHandler);
            EventService.AddListener<DieEnemyEventData>(DieEnemyHandler);
            EventService.AddListener(EventsDefinition.KILL_ALL_ENEMIES_ID, KillAllEnemiesHandler);
        }

        private void KillAllEnemiesHandler()
        {
            foreach (var l_enemy in m_enemies)
            {
                l_enemy.HealthController.TakeDamage(l_enemy.HealthController.GetMaxHealth());
            }
        }

        private void OnSpawnBossInRoomHandler(SpawnBossInRoom p_data)
        {
            m_currentRoom = p_data.Room;


            for (int i = 0; i < m_currentRoom.SpawnPoints.Count; i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[i];
                var rndBoss = p_data.Bosses[i];
                m_enemies.Add(Instantiate(rndBoss, l_spawnPoint.position, Quaternion.identity));
            }
        }

        private void OnDisable()
        {
            EventService.RemoveListener<SpawnEnemiesInRoomEventData>(SpawnEnemiesInRoomHandler);
            EventService.RemoveListener<SpawnEnemyEventData>(SpawnEnemyHandler);
            EventService.RemoveListener<SpawnBossInRoom>(OnSpawnBossInRoomHandler);
            EventService.RemoveListener<DieEnemyEventData>(DieEnemyHandler);
            EventService.RemoveListener(EventsDefinition.KILL_ALL_ENEMIES_ID, KillAllEnemiesHandler);
        }

        private void SpawnEnemiesInRoomHandler(SpawnEnemiesInRoomEventData p_data)
        {
            m_currentRoom = p_data.Room;
            if(m_currentRoom.SpawnPoints.Count == 0)
                return;
            
            m_currentRoom = p_data.Room;
            m_enemies.Clear();
            var l_countSpawn = Random.Range(m_currentRoom.MinEnemySpawn, m_currentRoom.MaxEnemySpawn + 1);
            for (var l_i = 0; l_i < l_countSpawn; l_i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[Random.Range(0, m_currentRoom.SpawnPoints.Count)];
                var l_enemyPrefab = enemyPoolData.GetRandomEnemyPrefabFromPool();
                var l_enemy = Instantiate(l_enemyPrefab, l_spawnPoint.position, l_enemyPrefab.transform.rotation);
                l_enemy.SetEnemyRoom(m_currentRoom);
                m_enemies.Add(l_enemy);
            }
        }

        private void SpawnEnemyHandler(SpawnEnemyEventData p_data)
        {
            var l_enemy = Instantiate(p_data.EnemyModelPrefab, p_data.SpawnPoint, Quaternion.identity);
            l_enemy.OnDie += DieEnemyHandler;
            l_enemy.SetEnemyRoom(p_data.Room);
            m_enemies.Add(l_enemy);
            
        }
        private void DieEnemyHandler(EnemyModel p_enemyModel)
        {
            if(m_enemies.Contains(p_data.Model))
                m_enemies.Remove(p_data.Model);

            if (m_enemies.Count <= 0)
                m_currentRoom.ClearRoom();
        }
    }

    public struct SpawnEnemiesInRoomEventData : ICustomEventData
    {
        public Room Room { get; }

        public SpawnEnemiesInRoomEventData(Room p_room)
        {
            Room = p_room;
        }
    }

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
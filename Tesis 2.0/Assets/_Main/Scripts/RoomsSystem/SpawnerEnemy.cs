using System.Linq;
using _Main.Scripts.Enemies;
using _Main.Scripts.ScriptableObjects;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;
using Random = UnityEngine.Random;

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
            EventService.AddListener<SpawnEnemiesInRoomEventData>(SpawnEnemiesInRoom);
            EventService.AddListener<SpawnEnemyEventData>(SpawnEnemy);
            EventService.AddListener<SpawnBossInRoom>(OnSpawnBossInRoom);
        }

        private void OnSpawnBossInRoom(SpawnBossInRoom p_data)
        {
            m_currentRoom = p_data.Room;
            
            m_enemyCount = 0;


            for (int i = 0; i < m_currentRoom.SpawnPoints.Count; i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[i];
                var rndBoss = p_data.Bosses[i];
                var l_enemy = Instantiate(rndBoss, l_spawnPoint.position, Quaternion.identity);
                m_enemyCount++;
                l_enemy.OnDie += DieEnemyHandler;
                
            }
            
        }

        private void OnDisable()
        {
            EventService.RemoveListener<SpawnEnemiesInRoomEventData>(SpawnEnemiesInRoom);
            EventService.RemoveListener<SpawnEnemyEventData>(SpawnEnemy);
            EventService.RemoveListener<SpawnBossInRoom>(OnSpawnBossInRoom);
        }

        private void SpawnEnemiesInRoom(SpawnEnemiesInRoomEventData p_data)
        {
            m_currentRoom = p_data.Room;
            if(m_currentRoom.SpawnPoints.Count == 0)
                return;
            
            m_enemyCount = 0;
            var l_countSpawn = Random.Range(m_currentRoom.MinEnemySpawn, m_currentRoom.MaxEnemySpawn + 1);
            for (var l_i = 0; l_i < l_countSpawn; l_i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[Random.Range(0, m_currentRoom.SpawnPoints.Count)];
                var l_enemyPrefab = enemyPoolData.GetRandomEnemyPrefabFromPool();
                var l_enemy = Instantiate(l_enemyPrefab, l_spawnPoint.position, l_enemyPrefab.transform.rotation);
                l_enemy.OnDie += DieEnemyHandler;
                l_enemy.SetEnemyRoom(m_currentRoom);
                m_enemyCount++;
            }
        }

        private void SpawnEnemy(SpawnEnemyEventData p_data)
        {
            var l_enemy = Instantiate(p_data.EnemyModelPrefab, p_data.SpawnPoint, Quaternion.identity);
            l_enemy.OnDie += DieEnemyHandler;
            l_enemy.SetEnemyRoom(p_data.Room);
            m_enemyCount++;
            
        }
        private void DieEnemyHandler(EnemyModel p_enemyModel)
        {
            p_enemyModel.OnDie -= DieEnemyHandler;
            m_enemyCount--;

            if (m_enemyCount <= 0)
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
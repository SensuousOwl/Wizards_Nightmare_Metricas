using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.ScriptableObjects;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventDatas;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using _Main.Scripts.StaticClass;
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
            EventService.AddListener<SpawnBossInRoomEventData>(OnSpawnBossInRoomHandler);
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

        private void OnSpawnBossInRoomHandler(SpawnBossInRoomEventData p_data)
        {
            m_currentRoom = p_data.Room;


            for (int i = 0; i < m_currentRoom.SpawnPoints.Count; i++)
            {
                var l_spawnPoint = m_currentRoom.SpawnPoints[i];
                var l_boss = p_data.Bosses[i];
                m_enemies.Add(Instantiate(l_boss, l_spawnPoint.position, Quaternion.identity));
                l_boss.SetEnemyRoom(p_data.Room);
            }
        }

        private void OnDisable()
        {
            EventService.RemoveListener<SpawnEnemiesInRoomEventData>(SpawnEnemiesInRoomHandler);
            EventService.RemoveListener<SpawnEnemyEventData>(SpawnEnemyHandler);
            EventService.RemoveListener<SpawnBossInRoomEventData>(OnSpawnBossInRoomHandler);
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
            l_enemy.SetEnemyRoom(p_data.Room);
            m_enemies.Add(l_enemy);
            
        }
        private void DieEnemyHandler(DieEnemyEventData p_data)
        {
            if(m_enemies.Contains(p_data.Model))
                m_enemies.Remove(p_data.Model);

            if (m_enemies.Count <= 0)
                m_currentRoom.ClearRoom();
        }
    }
}
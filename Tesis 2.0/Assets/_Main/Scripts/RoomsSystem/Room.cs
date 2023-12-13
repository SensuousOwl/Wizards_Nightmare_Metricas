using System;
using System.Collections.Generic;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Door> doors;
        [SerializeField] private Vector2 insideRoomSize;
        [field: SerializeField] public List<Transform> SpawnPoints { get; private set; }
        [field: SerializeField] public int MinEnemySpawn { get; private set; }
        [field: SerializeField] public int MaxEnemySpawn { get; private set; }

        private List<Door> m_doorsAvailable = new();
        private bool m_isClear;
        private bool m_isSpawnedEnemies;

        private static IEventService EventService => ServiceLocator.Get<IEventService>();

        public static event Action OnClearedRoom;
        
        private void Awake()
        {
            foreach (var l_door in doors)
            {
                l_door.OnActiveDoor += OnActiveDoorEventHandler;
                l_door.OnPlayerTeleport += OnActiveDoorEventHandler;
                m_doorsAvailable.Add(l_door);
            }
        }

        private void OnActiveDoorEventHandler()
        {
            var l_position = transform.position;
            var l_cameraTransform = Camera.main.transform;
            l_cameraTransform.position = new Vector3(l_position.x, l_position.y,
                l_cameraTransform.position.z);
            
            if (m_isClear || m_isSpawnedEnemies)
                return;
            
            EventService.DispatchEvent(new SpawnEnemyEventData(this));
            foreach (var l_door in doors)
            {
                l_door.SetOpenDoor(false);
            }
        }
        
        public void ClearRoom()
        {
            foreach (var l_door in doors)
            {
                l_door.SetOpenDoor(true);
            }
        }

        public bool IsOneDoorAvailable() => m_doorsAvailable == default || m_doorsAvailable.Count > 0;

        public bool TryGetDoorAvailable(out Door p_door)
        {
            p_door = default;
            if (m_doorsAvailable.Count <= 0)
                return false;
            p_door = m_doorsAvailable[Random.Range(0, m_doorsAvailable.Count)];
            return true;
        }

        private void OnActiveDoorEventHandler(Door p_door)
        {
            m_doorsAvailable.Remove(p_door);
            p_door.OnActiveDoor -= OnActiveDoorEventHandler;

            if (m_doorsAvailable.Count <= 0)
                m_doorsAvailable = default;
        }

        public bool IsInsideBounds(Vector2 pos)
        {
            var btmLeft = transform.position - (Vector3)insideRoomSize / 2;
            var topRight = transform.position + (Vector3)insideRoomSize / 2;
            
            if (pos.y > topRight.y)
                return false;
            if (pos.y < btmLeft.y)
                return false;
            if (pos.x > topRight.x)
                return false;
            if (pos.x < btmLeft.x)
                return false;

            return true;
        }
        
#if UNITY_EDITOR
        [ContextMenu("Get All Doors")]
        private void GetAllDoors()
        {
            doors = new List<Door>(GetComponentsInChildren<Door>());
        }
#endif
    }
}
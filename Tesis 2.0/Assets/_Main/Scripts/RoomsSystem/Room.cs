using System.Collections.Generic;
using Services;
using Services.MicroServices.EventsServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Door> doors;
        [field: SerializeField] public List<Transform> SpawnPoints { get; private set; }
        [field: SerializeField] public int MinEnemySpawn { get; private set; }
        [field: SerializeField] public int MaxEnemySpawn { get; private set; }

        private List<Door> m_doorsAvailable = new();
        private bool m_isClear;

        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        
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
            if (m_isClear)
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
        
#if UNITY_EDITOR
        [ContextMenu("Get All Doors")]
        private void GetAllDoors()
        {
            doors = new List<Door>(GetComponentsInChildren<Door>());
        }
#endif
    }
}
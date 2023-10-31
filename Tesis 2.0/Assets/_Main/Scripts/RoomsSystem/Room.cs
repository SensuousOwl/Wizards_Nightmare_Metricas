using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Door> doors;

        private List<Door> m_doorsAvailable = new();
        
        private void Awake()
        {
            foreach (var l_door in doors)
            {
                l_door.OnActiveDoor += OnActiveDoorEventHandler;
                m_doorsAvailable.Add(l_door);
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
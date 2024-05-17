using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Attributes;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.RoomsSystems
{
    [CreateAssetMenu(menuName = "Main/Rooms/RoomsPool")]
    public class RoomsPool : ScriptableObject
    {
        [field: SerializeField] public Room FirstRoomPrefab { get; private set; }
        [field: SerializeField] public Room BossRoomPrefab { get; private set; }
        [SerializeField] private List<Room> roomsPrefabs;
        [SerializeField] private List<float> roomsChances;
        private RouletteWheel<Room> m_roomsWheel;

        public Room GetRandomItemFromPool()
        {
            m_roomsWheel ??= new RouletteWheel<Room>(roomsPrefabs, roomsChances);

            return m_roomsWheel.RunWithCached();
        }


#if UNITY_EDITOR
        [Header("Only Editor")]
        [ReadOnlyInspector, SerializeField] public List<float> chancePercentage = new();
        

        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = roomsPrefabs.Distinct().ToList();
            roomsPrefabs = l_newList;

            if (roomsPrefabs.Count != roomsChances.Count)
            {
                while (roomsPrefabs.Count > roomsChances.Count)
                {
                    roomsChances.Add(0);
                }

                while (roomsPrefabs.Count < roomsChances.Count)
                {
                    roomsChances.RemoveAt(roomsChances.Count - 1);
                }
            }

            CheckItemPercentage();
        }

        [ContextMenu("Show item percentage")]
        public void CheckItemPercentage()
        {
            var l_totalChance = roomsChances.Sum();

            chancePercentage.Clear();

            for (int i = 0; i < roomsChances.Count; i++)
            {
                chancePercentage.Add((roomsChances[i] / l_totalChance) * 100);
            }
        }

        [ContextMenu("Clear percentage")]
        public void ClearPercentage()
        {
            chancePercentage.Clear();
        }
#endif
    }
}
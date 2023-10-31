using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Main.Scripts.ScriptableObjects.RoomsSystems;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class RoomsGenerator : MonoBehaviour
    {
        [SerializeField] private RoomsPool roomPool;
        [SerializeField] private Vector3 roomSizes;
        [SerializeField] private Vector2 firstRoomPosition;
        [SerializeField] private int minRoomCountToSpawn, maxRoomCountToSpawn;
        [SerializeField] private LayerMask roomsLayer;

        private List<Room> m_rooms = new();

#if UNITY_EDITOR
        [Header("Only Editor")]
        [SerializeField] private Transform content;
#endif
        
        private void Start()
        {
            GenerateRooms();
        }

        [ContextMenu("TestGenerate")]
        private async void GenerateRooms()
        {
            var l_firstRoom = Instantiate(roomPool.FirstRoomPrefab, firstRoomPosition, roomPool.FirstRoomPrefab.transform.rotation);
            m_rooms.Add(l_firstRoom);
            
            var l_roomCountToSpawn = Random.Range(minRoomCountToSpawn, maxRoomCountToSpawn + 1);
            for (var l_i = 0; l_i < l_roomCountToSpawn; l_i++)
            {
                await InstantiateRoom();
            }

            var l_maxDistance = 0f;
            var l_room = l_firstRoom;

            for (var l_i = 1; l_i < m_rooms.Count; l_i++)
            {
                var l_possibleRoom = m_rooms[l_i];
                if (!l_possibleRoom.IsOneDoorAvailable())
                    continue;
                
                var l_distance = Vector2.Distance(l_room.transform.position, l_possibleRoom.transform.position);
                
                if (l_distance < l_maxDistance)
                    continue;

                l_maxDistance = l_distance;
                l_room = l_possibleRoom;
            }

            l_room.TryGetDoorAvailable(out var l_doorToConnect);
            var l_dirToMove = GetDirToMove(l_doorToConnect.GetDoorDir());
            var l_roomToConnectPosition = l_room.transform.position;
            var l_position = l_roomToConnectPosition + (Vector3)(l_dirToMove * roomSizes);
            var l_bossRoom = Instantiate(roomPool.BossRoomPrefab, l_position, Quaternion.identity);

#if UNITY_EDITOR
            l_firstRoom.transform.parent = content;
            l_bossRoom.transform.parent = content;
#endif
            
            m_rooms = null;
        }

        private Task InstantiateRoom()
        {
            var l_roomToConnect = m_rooms[Random.Range(0, m_rooms.Count)];
            var l_watchDogTryGetDoorAvailable = 10000;
            Door l_doorToConnect;

            while (!l_roomToConnect.IsOneDoorAvailable() || !l_roomToConnect.TryGetDoorAvailable(out l_doorToConnect))
            {
                l_roomToConnect = m_rooms[Random.Range(0, m_rooms.Count)];
                l_watchDogTryGetDoorAvailable--;
                
                if (l_watchDogTryGetDoorAvailable > 0) 
                    continue;
                
                Debug.LogError("Room available not found");
                return Task.CompletedTask;
            }

            var l_dirToMove = GetDirToMove(l_doorToConnect.GetDoorDir());
            var l_roomToConnectPosition = l_roomToConnect.transform.position;
            var l_position = l_roomToConnectPosition + (Vector3)(l_dirToMove * roomSizes);
            Debug.Log(l_position);

            var l_watchDogOverlap = 10000;
            while (Physics.CheckBox(l_position, roomSizes / 2, Quaternion.identity, roomsLayer))
            {
                if (l_watchDogOverlap < 5000)
                {
                    l_roomToConnect = m_rooms[Random.Range(0, m_rooms.Count)];
                    l_roomToConnectPosition = l_roomToConnect.transform.position;
                }
                
                while (!l_roomToConnect.IsOneDoorAvailable() || !l_roomToConnect.TryGetDoorAvailable(out l_doorToConnect))
                {
                    l_roomToConnect = m_rooms[Random.Range(0, m_rooms.Count)];
                    l_watchDogTryGetDoorAvailable--;
                    
                    if (l_watchDogTryGetDoorAvailable > 0) 
                        continue;
                    
                    Debug.LogError("Room available not found");
                    return Task.CompletedTask;
                }

                l_dirToMove = GetDirToMove(l_doorToConnect.GetDoorDir());
                l_position = l_roomToConnectPosition + (Vector3)(l_dirToMove * roomSizes);
                l_watchDogOverlap--;

                if (l_watchDogOverlap > 0) 
                    continue;
                
                Debug.LogError("Room available not found");
                return Task.CompletedTask;
            }
            var l_newRoom = Instantiate(roomPool.GetRandomItemFromPool(), l_position, quaternion.identity);
            
            m_rooms.Add(l_newRoom);
            
            
#if UNITY_EDITOR
            l_newRoom.transform.parent = content;
#endif
            
            return Task.CompletedTask;
        }

        private static Vector2 GetDirToMove(DoorDirections p_doorDirection)
        {
            return p_doorDirection switch
            {
                DoorDirections.Right => Vector2.right,
                DoorDirections.Left => Vector2.left,
                DoorDirections.Top => Vector2.up,
                DoorDirections.Down => Vector2.down,
                _ => throw new ArgumentOutOfRangeException(nameof(p_doorDirection), p_doorDirection, null)
            };
        }
    }
}

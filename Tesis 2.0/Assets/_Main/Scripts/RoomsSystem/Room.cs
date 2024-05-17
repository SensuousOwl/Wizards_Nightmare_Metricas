using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.RoomsSystem.Grid;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventDatas;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using _Main.Scripts.StaticClass;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.RoomsSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Door> doors;
        public Vector2 InsideRoomSize = new Vector2(32,16);
        [field: SerializeField] public List<Transform> SpawnPoints { get; private set; }
        [field: SerializeField] public int MinEnemySpawn { get; private set; }
        [field: SerializeField] public int MaxEnemySpawn { get; private set; }
        [SerializeField] private bool isStartRoom;
        [SerializeField, Range(0, 100)] private float spawnInstantItemProbability = 8;
        [SerializeField] private bool alwaysOpen;

        [Header("Minimap")] 
        [SerializeField] private GameObject minimapViewCurr;
        [SerializeField] private GameObject minimapViewVisited;
            
        private List<Door> m_doorsAvailable = new();
        private bool m_isClear;
        
        protected static IEventService EventService => ServiceLocator.Get<IEventService>();
        protected static ISpawnItemsService SpawnItemsService => ServiceLocator.Get<ISpawnItemsService>();

        public MyNodeGrid Grid => roomGrid;
        [SerializeField] private MyNodeGrid roomGrid;
        
        
        private void Awake()
        {
            foreach (var l_door in doors)
            {
                l_door.OnDoorConnection += OnDoorConnectionEventHandler;
                l_door.OnTeleportPlayer += OnTeleportActiveDoorEventHandler;
                l_door.OnPlayerExitRoom+= DeactivateMinimapView;
                m_doorsAvailable.Add(l_door);
            }
        }

        

        private void Start()
        {
            StartCoroutine(ShowMinimap());
        }

        private IEnumerator ShowMinimap()
        {
            yield return new WaitForSeconds(0.5f);
                
            if (isStartRoom)
            {
                minimapViewCurr.SetActive(true);
                minimapViewVisited.SetActive(true);

                SetDoorsMinimapView(true);
            }
            else
            {
                minimapViewCurr.SetActive(false);
                minimapViewVisited.SetActive(false);
                SetDoorsMinimapView(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var l_door in doors)
            {
                l_door.OnDoorConnection -= OnDoorConnectionEventHandler;
                l_door.OnTeleportPlayer -= OnTeleportActiveDoorEventHandler;
                l_door.OnPlayerExitRoom -= DeactivateMinimapView;
            }
        }

        private void OnTeleportActiveDoorEventHandler()
        {
            var l_position = transform.position;
            var l_cameraTransform = Camera.main.transform;
            l_cameraTransform.position = new Vector3(l_position.x, l_position.y,
                l_cameraTransform.position.z);
            
            minimapViewCurr.SetActive(true);
            minimapViewVisited.SetActive(true);
            SetDoorsMinimapView(true);
            
            if (m_isClear || isStartRoom)
                return;

            SpawnEnemiesInRoom();
        }

        protected virtual void SpawnEnemiesInRoom()
        {
            EventService.DispatchEvent(new SpawnEnemiesInRoomEventData(this));
            EventService.DispatchEvent(EventsDefinition.ENTER_UNCLEAR_ROOM_ID);
            if(!alwaysOpen)
                CloseDoors();
        }

        protected void CloseDoors()
        {
            foreach (var l_door in doors)
            {
                l_door.SetOpenDoor(false);
            }
        }

        protected void OpenDoor()
        {
            foreach (var l_door in doors)
            {
                l_door.SetOpenDoor(true);
            }
        }
        
        public virtual void ClearRoom()
        {
            EventService.DispatchEvent(EventsDefinition.CLEAR_ROOM_ID);
            TrySpawnPickUp();
            m_isClear = true;
            OpenDoor();
        }

        private void TrySpawnPickUp()
        {
            var l_rnd = Random.Range(0, 100f);
            if (l_rnd < spawnInstantItemProbability)
            {
                SpawnItemsService.SpawnInstantItemRandom(transform.position);
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

        private void OnDoorConnectionEventHandler(Door p_door)
        {
            m_doorsAvailable.Remove(p_door);
            p_door.OnDoorConnection -= OnDoorConnectionEventHandler;
            
            if (m_doorsAvailable.Count <= 0)
                m_doorsAvailable = default;
        }

        public bool IsInsideBounds(Vector2 p_pos)
        {
            var l_btmLeft = transform.position - (Vector3)InsideRoomSize / 2;
            var l_topRight = transform.position + (Vector3)InsideRoomSize / 2;
            
            if (p_pos.y > l_topRight.y)
                return false;
            if (p_pos.y < l_btmLeft.y)
                return false;
            if (p_pos.x > l_topRight.x)
                return false;
            if (p_pos.x < l_btmLeft.x)
                return false;

            return true;
        }

        private void SetDoorsMinimapView(bool p_b)
        {
            foreach (var l_door in doors)
            {
                l_door.SetMinimapView(p_b);
            }
        }
        
        private void DeactivateMinimapView()
        {
            minimapViewCurr.SetActive(false);
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
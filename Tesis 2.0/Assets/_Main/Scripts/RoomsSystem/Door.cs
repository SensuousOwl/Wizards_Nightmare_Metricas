using System;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.RoomsSystem
{
    public enum DoorDirections
    {
        Right,
        Left,
        Top,
        Down
    }
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorDirections doorDirection;
        [SerializeField] private GameObject doorVisualClosed;
        [SerializeField] private GameObject doorVisualOpen;
        [SerializeField] private GameObject wallVisual;
        [SerializeField] private Transform playerSpawnTransform;
        [SerializeField] private GameObject boxCollider;

        
        [Header("Minimap")] 
        [SerializeField] private GameObject minimapVisual;
        private Door m_doorConnect;
        private bool m_isAvailable = true;
        private bool m_isOpen;
        private bool m_temporalyClose;
        private bool m_showDoor;
        
        private Room m_roomParent;
        public DoorDirections GetDoorDir() => doorDirection;
        private static readonly WaitForSeconds WaitForSeconds = new(2f);

        public event Action<Door> OnDoorConnection;
        public event Action OnTeleportPlayer;
        public event Action OnPlayerExitRoom;

        private void Awake()
        {
            doorVisualClosed.SetActive(false);
            doorVisualOpen.SetActive(false);
            wallVisual.SetActive(true);
            minimapVisual.SetActive(false);
            
        }

        private void ConnectDoorToAnotherRoom(Door p_doorToConnect)
        {
            m_doorConnect = p_doorToConnect;
            m_isAvailable = false;
            m_temporalyClose = false;

            m_showDoor = true;
            doorVisualClosed.SetActive(true);
            wallVisual.SetActive(false);
            
                
            OnDoorConnection?.Invoke(this);
            SetOpenDoor(true);
        }

        public void TeleportPlayer(Transform p_player)
        {
            StartCoroutine(DeactivateDoorTemporality());
            p_player.position = playerSpawnTransform.position;
            
            OnTeleportPlayer?.Invoke();
        }

        private IEnumerator DeactivateDoorTemporality()
        {
            m_temporalyClose = true;
            yield return WaitForSeconds;
            m_temporalyClose = false;
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (m_isAvailable && p_other.TryGetComponent(out Door l_door))
                ConnectDoorToAnotherRoom(l_door);
            
            if (!m_isOpen || m_temporalyClose)
                return;
            
            if (!p_other.CompareTag("Player")) 
                return;

            if (m_doorConnect == default) 
                return;
            
            OnPlayerExitRoom?.Invoke();
            StartCoroutine(DeactivateDoorTemporality());
            m_doorConnect.TeleportPlayer(p_other.transform);
        }
        public void SetOpenDoor(bool p_newValue)
        {
            m_isOpen = p_newValue;
            if (m_showDoor)
            {
                doorVisualOpen.SetActive(p_newValue);
                doorVisualClosed.SetActive(!p_newValue);
            }
            
            boxCollider.SetActive(!m_isOpen);
        }

        public void SetMinimapView(bool p_b)
        {
            if(m_showDoor)
                minimapVisual.SetActive(p_b);
        }

        
        public void SetRoomParent(Room p_room) => m_roomParent = p_room;
    }
}
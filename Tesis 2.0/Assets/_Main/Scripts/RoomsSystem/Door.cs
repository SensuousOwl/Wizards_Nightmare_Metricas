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
        [SerializeField] private GameObject doorVisual;
        [SerializeField] private GameObject wallVisual;
        [SerializeField] private Transform playerSpawnTransform;
        [SerializeField] private GameObject boxCollider;

        private Door m_doorConnect;
        private bool m_isAvailable = true;
        private bool m_isOpen;
        private bool m_temporalyClose;

        public DoorDirections GetDoorDir() => doorDirection;
        private static readonly WaitForSeconds WaitForSeconds = new(2f);

        public event Action<Door> OnActiveDoor;
        public event Action OnPlayerTeleport;

        private void Awake()
        {
            doorVisual.SetActive(false);
            wallVisual.SetActive(true);
        }

        private void ConnectDoor(Door p_doorToConnect)
        {
            m_doorConnect = p_doorToConnect;
            m_isAvailable = false;
            m_temporalyClose = false;
            
            doorVisual.SetActive(true);
            wallVisual.SetActive(false);
            OnActiveDoor?.Invoke(this);
            SetOpenDoor(true);
        }

        public void TeleportPlayer(Transform p_player)
        {
            StartCoroutine(DeactivateDoorTemporality());
            p_player.position = playerSpawnTransform.position;
            OnPlayerTeleport?.Invoke();
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
                ConnectDoor(l_door);
            
            if (!m_isOpen || m_temporalyClose)
                return;
            
            if (!p_other.CompareTag("Player")) 
                return;

            if (m_doorConnect == default) 
                return;

            StartCoroutine(DeactivateDoorTemporality());
            m_doorConnect.TeleportPlayer(p_other.transform);
        }
        public void SetOpenDoor(bool p_newValue)
        {
            m_isOpen = p_newValue;
            boxCollider.SetActive(!m_isOpen);
        }
    }
}
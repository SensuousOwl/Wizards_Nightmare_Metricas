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
        [SerializeField] private BoxCollider2D boxCollider;

        private Door m_doorConnect;
        private bool m_isAvailable = true;
        private bool m_isOpen;

        public DoorDirections GetDoorDir() => doorDirection;
        private static readonly WaitForSeconds WaitForSeconds = new(2f);

        public event Action<Door> OnActiveDoor;

        private void Awake()
        {
            doorVisual.SetActive(false);
            wallVisual.SetActive(true);
            SetActiveDoor(false);
        }

        private void ConnectDoor(Door p_doorToConnect)
        {
            m_doorConnect = p_doorToConnect;
            m_isAvailable = false;
            m_isOpen = true;
            
            //doorVisual.SetActive(true);
            wallVisual.SetActive(false);
            SetActiveDoor(true);
            OnActiveDoor?.Invoke(this);
        }

        public void SetActiveDoor(bool p_isOpen)
        {
            boxCollider.isTrigger = p_isOpen;
            StartCoroutine(DeactivateDoorTemporality());
        }

        public void TeleportPlayer(Transform p_player)
        {
            StartCoroutine(DeactivateDoorTemporality());
            p_player.position = playerSpawnTransform.position;
        }

        private IEnumerator DeactivateDoorTemporality()
        {
            m_isOpen = false;
            yield return WaitForSeconds;
            m_isOpen = true;
            boxCollider.isTrigger = !boxCollider.isTrigger;
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (!m_isOpen)
                return;
            
            if (m_isAvailable && p_other.TryGetComponent(out Door l_door))
                ConnectDoor(l_door);
            
            if (!p_other.CompareTag("Player")) 
                return;

            if (m_doorConnect == default) 
                return;
            
            StartCoroutine(DeactivateDoorTemporality());
            m_doorConnect.TeleportPlayer(p_other.transform);
        }
    }
}
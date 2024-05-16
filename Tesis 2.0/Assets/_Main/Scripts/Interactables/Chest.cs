using System;
using _Main.Scripts.Attributes;
using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using UnityEngine;

namespace _Main.Scripts.Interactables
{
    public class Chest : MonoBehaviour, IInteract
    {
        [SerializeField] private Transform itemSpawnPoint;
        [SerializeField] private GameObject interactVisual;
        [SerializeField] private GameObject visualOpen;
        [SerializeField] private GameObject visualClosed;

        [ReadOnlyInspector] private bool m_isOpen;

        private static ISpawnItemsService SpawnItemsService => ServiceLocator.Get<ISpawnItemsService>();

        private void Awake()
        {
            interactVisual.SetActive(false);
            visualOpen.SetActive(false);
            visualClosed.SetActive(true);
        }

        public void Interact()
        {
            if (m_isOpen)
                return;
            
            SpawnItemsService.SpawnItemChestRandom(itemSpawnPoint.position);
            m_isOpen = true;
            visualOpen.SetActive(true);
            visualClosed.SetActive(false);
        }
        public void ShowCanvasUI(bool p_b)
        {
            interactVisual.SetActive(p_b);
        }

        private void OnTriggerEnter(Collider p_other)
        {
            if (m_isOpen)
                return;
            
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(true);
            }
        }

        private void OnTriggerExit(Collider p_other)
        {
            if (m_isOpen)
                return;
            
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(false);
            }
        }
    }
}
using System;
using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Interactables
{
    public class ShowCreditsUiInteractable : MonoBehaviour, IInteract
    {
        [SerializeField] private GameObject uiGameObject;
        [SerializeField] private GameObject interactVisual;

        private void Awake()
        {
            interactVisual.SetActive(false);
        }

        public void Interact(PlayerModel p_model)
        {
            uiGameObject.SetActive(true);
        }
        
        
        public void ShowCanvasUI(bool p_b)
        {
            interactVisual.SetActive(p_b);
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(true);
            }
        }

        private void OnTriggerExit2D(Collider2D p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(false);
            }
        }
    }
}
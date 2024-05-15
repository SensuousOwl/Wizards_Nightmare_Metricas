using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Interactables
{
    public class ShowCreditsUiInteractable : MonoBehaviour, IInteract
    {
        [SerializeField] private GameObject uiGameObject;
        [SerializeField] private GameObject interactVisual;
        public void Interact(PlayerModel p_model)
        {
            uiGameObject.SetActive(true);
            //desabilitar el control del
        }
        
        
        public void ShowCanvasUI(bool p_b)
        {
            interactVisual.SetActive(p_b);
        }

        private void OnTriggerEnter(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(true);
            }
        }

        private void OnTriggerExit(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(false);
            }
        }
    }
}
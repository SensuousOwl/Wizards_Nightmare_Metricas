using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.RoomsSystem
{
    public class LadderController : MonoBehaviour, IInteract
    {
        [SerializeField] private string levelToPass;
        [SerializeField] private GameObject interactVisual;
        public void Interact(PlayerModel p_model)
        {
            SceneManager.LoadScene(levelToPass);
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
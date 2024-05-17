using System;
using _Main.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.RoomsSystem
{
    public class LadderController : MonoBehaviour, IInteract
    {
        [SerializeField] private string levelToPass;
        [SerializeField] private GameObject interactVisual;
        private void Awake()
        {
            interactVisual.SetActive(false);
        }
        public void Interact()
        {
            SceneManager.LoadScene(levelToPass);
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

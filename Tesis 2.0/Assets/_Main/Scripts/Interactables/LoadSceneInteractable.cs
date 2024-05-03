using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.Interactable
{
    public class LoadSceneInteractable : MonoBehaviour, IInteract
    {
        [SerializeField] private string sceneToLoad;
        public void Interact(PlayerModel p_model)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.RoomsSystem
{
    public class LadderController : MonoBehaviour, IInteract
    {
        [SerializeField] private string levelToPass;
        public void Interact(PlayerModel p_model)
        {
            SceneManager.LoadScene(levelToPass);
        }
    }
}
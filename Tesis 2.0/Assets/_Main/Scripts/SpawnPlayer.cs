using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerPrefab;

       
  
        private void Awake()
        {
            Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            InputManager.Instance.ChangeActionMap("Default-Keyboard");

            ExperienceController.Instance.StartRunTimer(); // Notificar inicio de la partida

            Destroy(gameObject);
        }

       
    }
}
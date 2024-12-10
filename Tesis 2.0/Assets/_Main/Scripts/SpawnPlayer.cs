using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerPrefab;


        //Metrica Time_On_Run_End y Enemies_Eliminated_On_Run_End
        private void StartNewRun()
        {
            ExperienceController.Instance.ResetEnemyEliminatedCount();
            ExperienceController.Instance.StartRunTimer();
        }

        private void Awake()
        {
            Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            InputManager.Instance.ChangeActionMap("Default-Keyboard");

            ExperienceController.Instance.StartRunTimer(); // Notificar inicio de la partida

            Destroy(gameObject);
        }

       
    }
}
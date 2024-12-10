using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.UI.Menus;
using UnityEngine;
using Unity.Services.Analytics;
using System.Collections.Generic;

namespace _Main.Scripts
{
    public class ExperienceController : MonoBehaviour
    {
        [SerializeField] private int countToNewLevel = 3;
        [SerializeField] private float experienceToUpgradeLevel = 100f;
        [SerializeField] private UpgradeScreenController upgradeScreenController;

        private float m_experience;
        private int m_level;
        private int m_newLevel = 1;

        private float runStartTime;
        public static ExperienceController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Permitir que persista entre escenas
        }

        private void OnEnable()
        {
            EnemyModel.OnExperienceDrop += EnemyModelOnOnDie;
        }

        private void OnDisable()
        {
            EnemyModel.OnExperienceDrop -= EnemyModelOnOnDie;
        }
        
        private void EnemyModelOnOnDie(float p_experienceDrop)
        {
            m_experience += p_experienceDrop;
            if (m_experience < experienceToUpgradeLevel)
                return;
            
            m_experience -= experienceToUpgradeLevel;
            experienceToUpgradeLevel += experienceToUpgradeLevel * 0.1f;
            m_level++;
            
            if (m_level < m_newLevel)
                 return;
            
            m_newLevel += countToNewLevel;
            upgradeScreenController.ActivateUpgradeScreen();
        }


        //Metrica Time_On_Run_End
        public void StartRunTimer()
        {
            runStartTime = Time.time;
            Debug.Log("Temporizador de la partida iniciado.");
        }

        public void EndRunTimer()
        {
            float runDuration = Time.time - runStartTime;

            // Registrar el evento en Unity Analytics
            var eventData = new Dictionary<string, object>
    {
        { "Run_Duration", runDuration } // Duración de la partida en segundos
    };

            AnalyticsService.Instance.CustomData("Time_On_Run_End", eventData);
            AnalyticsService.Instance.Flush(); // Envía los datos inmediatamente

            // Debug para revisar resultados
            Debug.Log($"Evento enviado: Time_On_Run_End con duración {runDuration} segundos.");

            // Verificar la duración
            if (runDuration < 600)
            {
                Debug.Log("La partida fue demasiado corta. Considera ajustar los enemigos o nerfear al jugador.");
            }
            else if (runDuration > 900)
            {
                Debug.Log("La partida fue demasiado larga. Considera ajustar los enemigos o balancear al jugador.");
            }
            else
            {
                Debug.Log("La duración de la partida está equilibrada.");
            }
        }
    }
}
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.UI.Menus;
using UnityEngine;
using Unity.Services.Analytics;
using System.Collections.Generic;
using _Main.Scripts.RoomsSystem;
using System;
using Unity.Services.Core;

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

        private int totalEnemiesEliminated;

        private int totalItemsPickedUp;

        private int currentLevel = 1;
        private int roomsCompleted;



        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
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


        //Metrica Time_On_Run_End y Enemies_Eliminated_On_Run_End
        public void StartRunTimer()
        {
            runStartTime = Time.time;
            Debug.Log("Temporizador de la partida iniciado.");
            Debug.Log($"Contador de enemigos eliminados reiniciado: {totalEnemiesEliminated}");
        }


        public void IncrementEnemyEliminatedCount()
        {
            totalEnemiesEliminated++;
            Debug.Log($"Enemigo eliminado. Total: {totalEnemiesEliminated}");
        }

        public int GetTotalEnemiesEliminated()
        {
            return totalEnemiesEliminated;
        }

        public void ResetEnemyEliminatedCount()
        {
            totalEnemiesEliminated = 0;
        }
        public void IncrementItemPickupCount()
        {
            totalItemsPickedUp++;
            Debug.Log($"Total de ítems recogidos: {totalItemsPickedUp}");
        }
        public void IncrementRoomsCompleted()
        {
            roomsCompleted++;
            Debug.Log($"Habitación completada. Total: {roomsCompleted}");
        }
        public void ResetRoomsCompleted()
        {
            roomsCompleted = 0;
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
            Debug.Log($"Nivel actual: {currentLevel}");
        }
        public void ResetItemPickupCount()
        {
            totalItemsPickedUp = 0;
        }
        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public float GetRunStartTime()
        {
            return runStartTime;
        }

        public void EndRunTimer(bool didWin)
        {
            if (runStartTime <= 0)
            {
                Debug.LogError("El temporizador de la partida no fue iniciado correctamente.");
                return;
            }

            // Calcular la duración de la partida
            float runDuration = Time.time - runStartTime;
            Debug.Log($"Partida finalizada. Duración: {runDuration} segundos. Enemigos eliminados: {totalEnemiesEliminated}");

            // Obtener datos desde RoomsGenerator
            int levelNumber = GetCurrentLevel();

            RoomsGenerator roomGen = FindObjectOfType<RoomsGenerator>();
            int roomsCompleted = roomGen != null ? roomGen.GetRoomsCompleted() : 0;
            // Validar datos antes de enviarlos
            if (runDuration <= 0)
            {
                Debug.LogError("La duración de la partida calculada es inválida (<= 0). Verifica que StartRunTimer() fue llamado correctamente.");
                return;
            }

            if (totalEnemiesEliminated < 0)
            {
                Debug.LogError("El conteo de enemigos eliminados es inválido (< 0). Asegúrate de que el contador se esté incrementando correctamente.");
                return;
            }

            // Enviar el evento Time_On_Run_End
            AnalyticsService.Instance.CustomData("Time_On_Run_End", new Dictionary<string, object>
    {
        { "RunDuration", runDuration } // Asegúrate de que este nombre sea compatible con el Dashboard
    });
            Debug.Log("Evento 'Time_On_Run_End' enviado correctamente.");

            // Enviar el evento Enemies_Eliminated_On_Run_End
            AnalyticsService.Instance.CustomData("Enemies_Eliminated_On_Run_End", new Dictionary<string, object>
    {
        { "EnemiesEliminated", totalEnemiesEliminated },
        { "Run_Duration", runDuration } // Agregado para correlacionar duración y enemigos eliminados
    });
            Debug.Log("Evento 'Enemies_Eliminated_On_Run_End' enviado correctamente.");

            // Enviar el evento Level_And_Room_Progression
            // Enviar el evento Level_And_Room_Progression con Outcome
            AnalyticsService.Instance.CustomData("Level_And_Room_Progression", new Dictionary<string, object>
    {
        { "RunDuration_", runDuration },
        { "RoomsCompleted", roomsCompleted },
        { "LevelNumber", GetCurrentLevel() },
        { "Outcome", didWin ? "Win" : "Loss" } // Nuevo parámetro
    });

            Debug.Log($"Evento 'Level_And_Room_Progression' enviado: RoomsCompleted={roomsCompleted}, LevelNumber={GetCurrentLevel()}, Outcome={(didWin ? "Win" : "Loss")}");



            // Forzar el envío inmediato de los datos a Unity Analytics
            AnalyticsService.Instance.Flush();
            ResetRoomsCompleted();
            ResetEnemyEliminatedCount();
            runStartTime = 0;
        }
    }
}



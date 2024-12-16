using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.UI.Menus;
using UnityEngine;
using Unity.Services.Analytics;
using System.Collections.Generic;
using _Main.Scripts.RoomsSystem;
using System;
using Unity.Services.Core;
using UnityEngine.Analytics;

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
        private int totalEnemiesEliminated;
        private int totalItemsPickedUp; // Contador de ítems recogidos
        private int roomsCompleted;
        private int currentLevel = 1;

        public static ExperienceController Instance { get; private set; }

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

        // MÉTRICAS
        public void StartRunTimer()
        {
            runStartTime = Time.time;
            totalEnemiesEliminated = 0;
            totalItemsPickedUp = 0;
            roomsCompleted = 0;
            Debug.Log("Temporizador y contadores reseteados al iniciar una nueva partida.");
        }

        public float GetRunStartTime()
        {
            return runStartTime;
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
            Debug.Log("Contador de enemigos eliminados reseteado.");
        }

        public void IncrementItemPickupCount()
        {
            totalItemsPickedUp++;
            Debug.Log($"Ítem recogido. Total: {totalItemsPickedUp}");
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
            Debug.Log($"Nivel actual actualizado: {currentLevel}");
        }

        public void IncrementRoomsCompleted()
        {
            RoomsGenerator roomGen = FindObjectOfType<RoomsGenerator>();
            if (roomGen != null)
            {
                int currentRoomsCompleted = roomGen.GetRoomsCompleted();
                Debug.Log($"Habitaciones completadas obtenidas desde RoomsGenerator: {currentRoomsCompleted}");
            }
            else
            {
                Debug.LogError("RoomsGenerator no encontrado.");
            }
        }

        public void ResetRoomsCompleted()
        {
            roomsCompleted = 0;
            Debug.Log("Contador de habitaciones completadas reseteado.");
        }

        public void EndRunTimer(bool didWin)
        {

            if (runStartTime <= 0)
            {
                Debug.LogError("El temporizador de la partida no fue iniciado correctamente.");
                return;
            }

            float runDuration = Time.time - runStartTime;
            RoomsGenerator roomGen = FindObjectOfType<RoomsGenerator>();
            int roomsCompleted = roomGen != null ? roomGen.GetRoomsCompleted() : 0;

            if (runDuration <= 0)
            {
                Debug.LogError("La duración de la partida es inválida (<= 0).");
                return;
            }

            Debug.Log($"Partida finalizada. Duración: {runDuration} segundos. Enemigos eliminados: {totalEnemiesEliminated}");

            AnalyticsService.Instance.CustomData("Time_On_Run_End", new Dictionary<string, object>
            {
                { "RunDuration", runDuration }
            });

            AnalyticsService.Instance.CustomData("Enemies_Eliminated_On_Run_End", new Dictionary<string, object>
            {
                { "EnemiesEliminated", totalEnemiesEliminated },
                { "Run_Duration", runDuration }
            });

            AnalyticsService.Instance.CustomData("Level_And_Room_Progression", new Dictionary<string, object>
{
              { "RunDuration_", runDuration },
              { "RoomsCompleted", roomsCompleted }, // Valor calculado
              { "LevelNumber", currentLevel },
              { "Outcome", didWin ? "Win" : "Loss" }
});

            ResetRunData();
        }

        private void ResetRunData()
        {
            runStartTime = 0;
            totalEnemiesEliminated = 0;
            totalItemsPickedUp = 0;
            roomsCompleted = 0;
            Debug.Log("Datos de la partida reseteados.");
        }
    }
}









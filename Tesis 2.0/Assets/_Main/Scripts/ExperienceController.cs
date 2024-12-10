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

        public void EndRunTimer()
        {
            // Validar si el temporizador de la partida fue iniciado
            if (runStartTime <= 0)
            {
                Debug.LogError("El temporizador de la partida no fue iniciado correctamente. Asegúrate de llamar a StartRunTimer() al inicio de la run.");
                return;
            }

            // Calcular la duración de la partida
            float runDuration = Time.time - runStartTime;
            Debug.Log($"Partida finalizada. Duración: {runDuration} segundos. Enemigos eliminados: {totalEnemiesEliminated}");

            // Validar que los datos sean razonables antes de enviarlos
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
        { "Run_Duration", runDuration }
    });
            Debug.Log("Evento 'Time_On_Run_End' enviado correctamente.");

            // Enviar el evento Enemies_Eliminated_On_Run_End
            AnalyticsService.Instance.CustomData("Enemies_Eliminated_On_Run_End", new Dictionary<string, object>
    {
        { "EnemiesEliminated", totalEnemiesEliminated }
    });
            Debug.Log("Evento 'Enemies_Eliminated_On_Run_End' enviado correctamente.");

            // Forzar el envío inmediato de los datos a Unity Analytics
            AnalyticsService.Instance.Flush();
            Debug.Log("Datos enviados a Unity Analytics.");

            // Reiniciar variables para la próxima partida
            ResetEnemyEliminatedCount();
            runStartTime = 0;
            Debug.Log("Estado reiniciado para la próxima partida.");
        }
    }
}


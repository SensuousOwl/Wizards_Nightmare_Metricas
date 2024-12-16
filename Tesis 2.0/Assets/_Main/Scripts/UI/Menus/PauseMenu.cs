using _Main.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;

namespace _Main.Scripts.UI.Menus
{
    public class PauseMenu : BasePanel
    {
        [SerializeField] private string mainMenuScene = "MainMenuScene";
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private SettingsPanel settingsPanel;
        [SerializeField] private InventoryPanel inventoryPanel;
    
        public void Start() //because the pause manager might not be alive in the awake, we do everything on the start
        {
            resumeButton.onClick.AddListener(ResumePlay);
            settingsButton.onClick.AddListener(OpenSettingsPanel);
            inventoryButton.onClick.AddListener(OpenInventoryPanel);
            mainMenuButton.onClick.AddListener(LoadMainMenu);
            quitButton.onClick.AddListener(QuitGame);

            settingsPanel.Initialize();
            settingsPanel.Close();
            
            inventoryPanel.Initialize();
            inventoryPanel.Close();
            
            InputManager.Instance.SubscribeInput("Inventory", OnInventoryPerformed);

            PauseManager.Instance.OnPause += SetPausePanel;
            Close();
        }

        private void QuitGame()
        {
            int levelNumber = ExperienceController.Instance.GetCurrentLevel();
            if (levelNumber <= 0)
            {
                Debug.LogWarning("LevelNumber no es válido. Configurando un valor predeterminado de 1.");
                levelNumber = 1;
            }

            float runDuration = Time.time - ExperienceController.Instance.GetRunStartTime();
            AnalyticsService.Instance.CustomData("Run_Abandoned", new Dictionary<string, object>
    {
        { "LevelNumber", levelNumber },
        { "RunnDuration", runDuration }
    });
            AnalyticsService.Instance.Flush();

            Debug.Log($"Evento 'Run_Abandoned' enviado: LevelNumber={levelNumber}, RunDuration={runDuration}");
            Application.Quit();
        }

        private void LoadMainMenu()
        {
            int levelNumber = ExperienceController.Instance.GetCurrentLevel();
            if (levelNumber <= 0)
            {
                Debug.LogWarning("LevelNumber no es válido. Configurando un valor predeterminado de 1.");
                levelNumber = 1;
            }

            float runDuration = Time.time - ExperienceController.Instance.GetRunStartTime();
            AnalyticsService.Instance.CustomData("Run_Abandoned", new Dictionary<string, object>
    {
        { "LevelNumber", levelNumber },
        { "RunnDuration", runDuration }
    });
            AnalyticsService.Instance.Flush();

            Debug.Log($"Evento 'Run_Abandoned' enviado: LevelNumber={levelNumber}, RunDuration={runDuration}");
            PauseManager.Instance.SetPause(false);
            SceneManager.LoadScene(mainMenuScene);
        }

        private void OnDisable()
        {
            InputManager.Instance.UnsubscribeInput("Inventory", OnInventoryPerformed);
        }
        
        private void OnInventoryPerformed(InputAction.CallbackContext p_obj)
        {
            OpenInventoryPanel();
        }

        private void OpenSettingsPanel()
        {
            settingsPanel.Open();
        }
        
        private void OpenInventoryPanel()
        {
            inventoryPanel.Open();
        }

        private void ResumePlay()
        {
            PauseManager.Instance.SetPause(false);
        }
    
        public void SetPausePanel(bool p_pauseState)
        {
            if (p_pauseState)
                Open();
            else
                Close();
        }
    }
}

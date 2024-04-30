using _Main.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

namespace _Main.Scripts.UI
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
            Application.Quit();
        }

        private void LoadMainMenu()
        {
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

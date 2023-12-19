using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Interfaces;
using _Main.Scripts.Managers;
using _Main.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BasePanel
{
    [SerializeField] private string mainMenuScene = "MainMenuScene";
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private SettingsPanel settingsPanel;
    
    public void Start() //because the pause manager might not be alive in the awake, we do everything on the start
    {
        resumeButton.onClick.AddListener(ResumePlay);
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        settingsPanel.Initialize();
        settingsPanel.Close();

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

    private void OpenSettingsPanel()
    {
        settingsPanel.Open();
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

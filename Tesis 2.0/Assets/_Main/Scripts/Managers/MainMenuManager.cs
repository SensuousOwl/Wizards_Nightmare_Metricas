using _Main.Scripts.UI.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Main.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private Button playButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button backButton;
        [SerializeField] private BasePanel creditsScreen;
        [SerializeField] private SettingsPanel settingsScreen;
        
        
        private void Awake()
        {
            creditsScreen.Close();
            settingsScreen.Close();
        
            playButton.onClick.AddListener(OnPlayButtonClicked);
            creditsButton.onClick.AddListener(OnCreditsButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);

            settingsScreen.Initialize();
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        private void OnCreditsButtonClicked()
        {
            creditsScreen.Open();
        }
        
        private void OnSettingsButtonClicked()
        {
            settingsScreen.Open();
        }

        private void OnBackButtonClicked()
        {
            if(creditsScreen.IsOpen)
                creditsScreen.Close();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}

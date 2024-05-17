using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class EndSceneController : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button exitButton;
    
        void Awake()
        {
            menuButton.onClick.AddListener(OnMenuButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}

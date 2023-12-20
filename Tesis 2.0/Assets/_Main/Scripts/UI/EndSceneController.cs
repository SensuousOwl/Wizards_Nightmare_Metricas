using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    
    // Start is called before the first frame update
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

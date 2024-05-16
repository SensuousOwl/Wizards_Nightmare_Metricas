using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _main.Scripts.Managers
{
    public class LoadSceneManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Image loadingBar;

        /*private void Start()
        {
            loadingText.enabled = false;
            loadingBar.fillAmount = 0f;

            StartCoroutine(LoadSceneAsyncCoroutine());
        }*/

        /*private IEnumerator LoadSceneAsyncCoroutine()
        {
            var l_gameManager = GameManager.Instance;
            if (l_gameManager == default)
            {
                Debug.LogError("NullReference GameManager in LoadSceneManager");
                yield break;
            }

            var l_sceneName = GameManager.Instance.GetNextSceneToLoad();

            if (l_sceneName.IsNullOrEmpty())
            {
                Debug.LogError("NextSceneToLoad is Null or Empty");
                yield break;
            }
            
            var l_asyncLoad = SceneManager.LoadSceneAsync(l_sceneName);

            while (!l_asyncLoad.isDone)
            {
                loadingText.enabled = true;
                var l_progress = Mathf.Clamp01(l_asyncLoad.progress / 0.9f);
                loadingText.text = $"Loading: {Mathf.RoundToInt(l_progress * 100)}%";
                loadingBar.fillAmount = l_progress;
                yield return null;
            }

            //Todo: replace new input system. (InputManager)
            loadingText.text = "Press any key to continue";
            while (!Input.anyKeyDown)
                yield return null;

            SceneManager.UnloadSceneAsync(l_gameManager.GetLoadSceneName());
        }*/
    }
}
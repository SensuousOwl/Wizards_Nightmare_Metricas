using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.LoadingSystem
{
    public class LoadSceneManager : MonoBehaviour
    {
        [SerializeField] private DialogueSystem dialogueSystem;
        [SerializeField] private string levelToLoad = "Level A_Scene";
        private bool m_isDialogueFinish;

        private void Awake()
        {
            dialogueSystem.OnDialogueFinish += DialogueSystemOnOnDialogueFinish;
        }

        private void OnDestroy()
        {
            if (dialogueSystem != default)
                dialogueSystem.OnDialogueFinish -= DialogueSystemOnOnDialogueFinish;
        }

        private void DialogueSystemOnOnDialogueFinish()
        {
            m_isDialogueFinish = true;
        }

        private void Start()
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            while (!m_isDialogueFinish)
            {
                yield return null;
            }

            SceneManager.LoadScene(levelToLoad);
        }
    }
}
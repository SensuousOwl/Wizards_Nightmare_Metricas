using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.LoadingSystem
{
    public class LoadSceneManager : MonoBehaviour
    {
        [SerializeField] private DialogueSystem dialogueSystem;
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

            var l_config = new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics2D);
            SceneManager.LoadScene("Level A_Scene", l_config);
        }
    }
}
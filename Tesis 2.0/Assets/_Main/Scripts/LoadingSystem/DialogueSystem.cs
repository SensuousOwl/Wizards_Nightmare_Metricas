using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Main.Scripts.LoadingSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI contentText;

        [SerializeField] private float typingTime;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<Sprite> background;
        [SerializeField, Multiline] private string[] dialogueParams;

        private WaitForSeconds m_typingTime;
        private Coroutine m_typeDialogueCoroutine;
        private List<string> m_listDialogue = new();
        private int m_index;
        private bool m_isFinish;

        public event Action OnDialogueFinish;

        private void Awake()
        {
            m_typingTime = new WaitForSeconds(typingTime);
            InputManager.Instance.SubscribeInput("SkipDialogue", OnSkipDialogueInputHandler);
            SetDialogueElements(dialogueParams);
            StartDialogue();
        }

        private void OnDestroy()
        {
            InputManager.Instance.UnsubscribeInput("SkipDialogue", OnSkipDialogueInputHandler);
        }

        private void SetDialogueElements(params string[] p_dialogueParams)
        {
            m_listDialogue = p_dialogueParams.ToList();
            m_index = 0;
        }

        private void StartDialogue()
        {
            if (m_typeDialogueCoroutine != null)
            {
                StopCoroutine(m_typeDialogueCoroutine);
            }

            m_typeDialogueCoroutine = StartCoroutine(Dialogue());
        }

        private IEnumerator Dialogue()
        {
            contentText.text = "";
            backgroundImage.sprite = background[m_index];

            foreach (var l_auxChar in m_listDialogue[m_index])
            {
                contentText.text += l_auxChar;
                yield return m_typingTime;
            }

            m_typeDialogueCoroutine = null;
            m_index++;
            m_isFinish = true;
        }

        private void OnSkipDialogueInputHandler(InputAction.CallbackContext p_obj)
        {
            if (m_isFinish)
            {
                m_isFinish = false;

                if (m_index < m_listDialogue.Count)
                    StartDialogue();
                else
                    DialogueFinish();
                return;
            }

            SkipDialogue();
        }

        private void DialogueFinish()
        {
            OnDialogueFinish?.Invoke();
        }

        private void SkipDialogue()
        {
            StopAllCoroutines();
            contentText.text = m_listDialogue[m_index];
            m_index++;

            m_isFinish = true;
        }
    }
}
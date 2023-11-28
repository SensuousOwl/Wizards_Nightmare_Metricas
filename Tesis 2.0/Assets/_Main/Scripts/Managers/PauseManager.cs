using System;
using System.Collections.Generic;
using _Main.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Managers
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;

        private Action<bool> OnPause;
        private bool m_isPause;

        private void Awake()
        {
            if (Instance != default)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            InputManager.Instance.SubscribeInput("Pause", OnPausePerformed);
        }

        private void OnDisable()
        {
            InputManager.Instance.UnsubscribeInput("Pause", OnPausePerformed);
        }

        private void OnPausePerformed(InputAction.CallbackContext p_obj)
        {
            m_isPause = !m_isPause;
            OnPause?.Invoke(m_isPause);
        }

        public void Subscribe(IPausable p_pausable)
        {
            OnPause += p_pausable.Pause;
        }
        
        public void Unsubscribe(IPausable p_pausable)
        {
            OnPause -= p_pausable.Pause;
        }
    }
}
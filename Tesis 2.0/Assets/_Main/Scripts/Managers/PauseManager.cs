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

        public event Action<bool> OnPause;
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
            SetPause(!m_isPause);
        }
        

        public void Subscribe(IPausable p_pausable)
        {
            OnPause += p_pausable.Pause;
        }
        
        public void Unsubscribe(IPausable p_pausable)
        {
            OnPause -= p_pausable.Pause;
        }

        public void SetPauseUpgrade(bool isPaused)
        {
            m_isPause = isPaused;
            Time.timeScale = m_isPause ? 0 : 1f;
        }

        public void SetPause(bool isPaused)
        {
            m_isPause = isPaused;
            Time.timeScale = m_isPause ? 0 : 1f;
            OnPause?.Invoke(m_isPause);
        }
    }
}
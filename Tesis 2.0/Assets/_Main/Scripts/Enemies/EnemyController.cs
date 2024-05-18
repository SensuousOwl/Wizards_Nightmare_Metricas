using _Main.Scripts.Attributes;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Interfaces;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Enemies
{
    public class EnemyController : MonoBehaviour, IPausable
    {
        private StateMachine m_stateMachine;
        [ReadOnlyInspector, SerializeField] private StateData currentState;
        private bool m_isPause;

        private void Start()
        {
            var l_model = GetComponent<EnemyModel>();
            m_stateMachine = new StateMachine(l_model);
        }

        private void Update()
        {
            if (m_isPause)
                return;
            
            m_stateMachine.RunStateMachine();


#if UNITY_EDITOR
            currentState = m_stateMachine.GetCurrentState();
#endif
        }

        public void SubscribePause()
        {
            PauseManager.Instance.Subscribe(this);
        }

        public void UnsubscribePause()
        {
            PauseManager.Instance.Unsubscribe(this);
        }

        public void Pause(bool p_pauseState)
        {
            m_isPause = p_pauseState;
        }
    }
}
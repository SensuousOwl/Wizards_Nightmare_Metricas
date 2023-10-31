using _Main.Scripts.Attributes;
using FSM.Base;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        private StateMachine m_stateMachine;
        [ReadOnlyInspector, SerializeField] private StateData currentState;
        private void Start()
        {
            var l_model = GetComponent<EnemyModel>();
            m_stateMachine = new StateMachine(l_model);
        }

        private void Update()
        {
            m_stateMachine.RunStateMachine();


#if UNITY_EDITOR
            
            currentState = m_stateMachine.GetCurrentState();
#endif
        }
    }
}
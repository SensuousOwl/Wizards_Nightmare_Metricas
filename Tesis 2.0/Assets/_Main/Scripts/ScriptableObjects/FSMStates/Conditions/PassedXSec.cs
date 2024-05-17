using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PassedXSec", menuName = "_main/States/Conditions/PassedXSec", order = 0)]
    public class PassedXSec : StateCondition
    {
        [SerializeField] private float time;
        private float m_currTime = 0f;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            m_currTime += Time.deltaTime;

            if (m_currTime > time)
            {
                m_currTime = 0;
                return true;
            }
            
            return false;
        }
    }
}
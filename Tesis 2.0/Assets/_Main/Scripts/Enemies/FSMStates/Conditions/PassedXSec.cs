using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
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
            else return false;
        }
    }
}
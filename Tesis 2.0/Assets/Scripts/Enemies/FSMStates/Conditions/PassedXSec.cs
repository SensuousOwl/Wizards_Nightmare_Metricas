using FSM.Base;
using UnityEngine;

namespace Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PassedXSec", menuName = "_main/States/Conditions/PassedXSec", order = 0)]
    public class PassedXSec : StateCondition
    {
        [SerializeField] private float time;
        private float m_currTime;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            m_currTime += Time.deltaTime;

            return m_currTime > time;
        }
    }
}
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "HpUnderZero", menuName = "_main/States/Conditions/HpUnderZero", order = 0)]
    public class HpUnderZero : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return p_model.HealthController.GetCurrentHealth() <= 0;
        }
    }
}
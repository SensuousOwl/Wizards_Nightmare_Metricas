using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "IsAttackingCondition", menuName = "_main/States/Conditions/IsAttackingCondition", order = 0)]
    public class IsAttackingCondition : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return p_model.IsAttacking;
        }
    }
}
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "AlwaysTrueCondition", menuName = "_main/States/Conditions/AlwaysTrueCondition", order = 0)]
    public class AlwaysTrueCondition : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return true;
        }
    }
}
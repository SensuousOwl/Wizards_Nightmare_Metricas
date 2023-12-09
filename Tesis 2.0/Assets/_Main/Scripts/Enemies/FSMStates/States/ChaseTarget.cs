using _Main.Scripts.FSM.Base;
using _Main.Scripts.Steering_Behaviours;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ChaseTarget", menuName = "_main/States/Executions/ChaseTarget", order = 0)]
    public class ChaseTarget : MyState
    {
        public override void ExecuteState(EnemyModel p_model)
        {
            p_model.MoveTowards(p_model.GetTargetTransform().position);
        }
    }
}
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "_main/States/Executions/IdleState", order = 0)]
    public class IdleMyState: MyState
    {
        public override void EnterState(EnemyModel p_model)
        {
            p_model.View.PlayIdleAnim();
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            
        }
    }
}
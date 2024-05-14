using _Main.Scripts.FSM.Base;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "BossDieAndDestroyState", menuName = "_main/States/Executions/BossDieAndDestroyState", order = 0)]
    public class BossDieAndDestroyState : MyState
    {
        public override void EnterState(EnemyModel p_model)
        {
            
            p_model.TriggerDieEvent();
            Destroy(p_model.gameObject);
        }
        public override void ExecuteState(EnemyModel p_model)
        {
        }
    }
}
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "DieAndDestroyState", menuName = "_main/States/Executions/DieAndDestroyState", order = 0)]
    public class DieAndDestroyState : MyState
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
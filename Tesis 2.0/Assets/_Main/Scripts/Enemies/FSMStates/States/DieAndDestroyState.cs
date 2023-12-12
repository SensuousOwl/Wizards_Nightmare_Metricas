using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "DieAndDestroyState", menuName = "_main/States/Executions/DieAndDestroyState", order = 0)]
    public class DieAndDestroyState : MyState
    {
        public override void EnterState(EnemyModel p_model)
        {
            p_model.TriggerDieEvent();
            // ExperienceController.Instance.EnemyModelOnOnDie(p_model);
            Destroy(p_model.gameObject);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
        }
    }
}
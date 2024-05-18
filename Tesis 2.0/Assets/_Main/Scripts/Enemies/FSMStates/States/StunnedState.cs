using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "StunnedState", menuName = "_main/States/Executions/StunnedState", order = 0)]
    public class StunnedState : MyState
    {
        [SerializeField] private string stunnedStateAnim; 
        public override void EnterState(EnemyModel p_model)
        {
            p_model.SetRbSpeed(Vector2.zero);
            p_model.View.PlayAnim(stunnedStateAnim);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
        }
    }
}
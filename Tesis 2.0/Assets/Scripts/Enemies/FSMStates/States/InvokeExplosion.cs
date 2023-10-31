using FSM.Base;
using UnityEngine;

namespace Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "InvokeExplosion", menuName = "_main/States/Executions/InvokeExplosion", order = 0)]
    public class InvokeExplosion : MyState
    {
        [SerializeField] private float explosionRadius;
        public override void EnterState(EnemyModel p_model)
        {
            //TODO, que haga la explosion y le haga 99999 de damage al EEnemyModel
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            throw new System.NotImplementedException();
        }
    }
}
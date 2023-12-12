using _Main.Scripts.FSM.Base;
using _Main.Scripts.Steering_Behaviours;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ChaseTarget", menuName = "_main/States/Executions/ChaseTarget", order = 0)]
    public class ChaseTargetState : MyState
    {
        [SerializeField] private LayerMask ObsMask;
        [SerializeField] private float avoidForce = 10f;
        public override void ExecuteState(EnemyModel p_model)
        {
            var data = p_model.GetData();

            var dir = MySteeringBehaviors.GetAdvancedObsAvoidanceDir(p_model.transform.position,
                p_model.GetTargetTransform().position, data.ObsDetectionRadius, avoidForce, ObsMask);
            p_model.Move(dir);
        }
    }
}
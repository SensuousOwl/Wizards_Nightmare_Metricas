using _Main.Scripts.FSM.Base;
using _Main.Scripts.Steering_Behaviours;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ChaseTarget", menuName = "_main/States/Executions/ChaseTarget", order = 0)]
    public class ChaseTarget : MyState
    {
        [SerializeField] private LayerMask ObsMask;
        [SerializeField] private float avoidForce = 10f;
        public override void ExecuteState(EnemyModel p_model)
        {
            var data = p_model.GetData();

            var obsAvoidDir = MySteeringBehaviors.GetObsAvoidanceDir(p_model.transform.position, p_model.CurrDir, data.ObsDetectionRadius,
                data.ObsDetectionAngle, ObsMask);
            
            var chaseDir = p_model.transform.position +
                MySteeringBehaviors.GetChaseDir(p_model.transform.position, p_model.GetTargetTransform().position);
            
            p_model.MoveTowards(obsAvoidDir*avoidForce + chaseDir);
        }
    }
}
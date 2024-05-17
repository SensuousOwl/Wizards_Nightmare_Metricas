using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "ChaseTarget", menuName = "_main/States/Executions/Movement/ChaseTarget", order = 0)]
    public class ChaseTargetState : MyState
    {
        [SerializeField] private LayerMask ObsMask;
        [SerializeField] private float avoidForce = 10f;
        public override void ExecuteState(EnemyModel p_model)
        {
            var l_data = p_model.GetData();

            var l_dir = MySteeringBehaviors.GetAdvancedObsAvoidanceDir(p_model.transform.position,
                PlayerModel.Local.transform.position, l_data.ObsDetectionRadius, avoidForce, ObsMask);
            
            p_model.Move(l_dir);
        }
    }
}
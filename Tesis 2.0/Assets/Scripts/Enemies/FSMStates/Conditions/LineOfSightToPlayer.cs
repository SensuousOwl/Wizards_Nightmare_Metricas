using FSM.Base;
using UnityEngine;

namespace Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "LineOfSightToPlayer", menuName = "_main/States/Conditions/LineOfSightToPlayer", order = 0)]
    public class LineOfSightToPlayer : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return LineOfSight(p_model);
        }
        
        private bool LineOfSight(EnemyModel p_model)
        {
            var lTargetPos = p_model.GetTargetTransform().position;
            var lModelTransform = p_model.transform;
            Vector3 lDirectionToTarget = lTargetPos - lModelTransform.position;
            
            float lDistanceToTarget = lDirectionToTarget.magnitude;

            if (lDistanceToTarget > p_model.GetData().ViewDepthRange) 
                return false;
           

            if (Physics.Linecast(lModelTransform.position, lTargetPos, p_model.GetData().TargetMask))
            {
                p_model.SetLastTargetLocation(lTargetPos);
                return true;
            }

            return false;
        }
    }
}
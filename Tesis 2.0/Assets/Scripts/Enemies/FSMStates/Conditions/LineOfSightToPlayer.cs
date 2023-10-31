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
            var l_lTargetPos = p_model.GetTargetTransform().position;
            var l_lModelTransform = p_model.transform;
            var l_lDirectionToTarget = l_lTargetPos - l_lModelTransform.position;
            
            var l_lDistanceToTarget = l_lDirectionToTarget.magnitude;

            if (l_lDistanceToTarget > p_model.GetData().ViewDepthRange) 
                return false;


            if (!Physics2D.Linecast(l_lModelTransform.position, l_lTargetPos, p_model.GetData().TargetMask))
                return false;
            
            p_model.SetLastTargetLocation(l_lTargetPos);
            return true;

        }
    }
}
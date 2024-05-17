using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
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
            var l_lTargetPos = PlayerModel.Local.transform.position;
            var l_lModelTransform = p_model.transform;
            var l_lDirectionToTarget = l_lTargetPos - l_lModelTransform.position;
            
            var l_lDistanceToTarget = l_lDirectionToTarget.magnitude;

            if (l_lDistanceToTarget > p_model.GetData().ViewDepthRange) 
                return false;


            return Physics2D.Linecast(l_lModelTransform.position, l_lTargetPos, p_model.GetData().TargetMask);
        }
    }
}
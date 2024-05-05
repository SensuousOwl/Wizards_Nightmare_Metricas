using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "HitWallCondition", menuName = "_main/States/Conditions/HitWallCondition", order = 0)]
    public class HitWallCondition : StateCondition
    {
        [SerializeField] private LayerMask wallMask;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var modelPos = p_model.transform.position;
            
            
            
            return Physics2D.CircleCast(modelPos,0.5f, Vector3.right,1, wallMask) ||
                   Physics2D.CircleCast(modelPos,0.5f, Vector3.left,1, wallMask);
        }
        
        
    }
}
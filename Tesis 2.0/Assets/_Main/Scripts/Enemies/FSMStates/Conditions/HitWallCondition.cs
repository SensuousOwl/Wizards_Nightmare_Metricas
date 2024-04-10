using _Main.Scripts.FSM.Base;
using UnityEditor.Experimental.GraphView;
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
            var rightModelPos = modelPos + Vector3.right;
            
            return Physics2D.Linecast(modelPos, rightModelPos, wallMask) ||
                   Physics2D.Linecast(modelPos, -rightModelPos, wallMask);
        }
        
        
    }
}
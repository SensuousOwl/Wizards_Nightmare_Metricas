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
            var l_modelPos = p_model.transform.position;

            var l_halfRight = new Vector2(l_modelPos.x + p_model.GetEnemySize().x / 2  + 0.15f, l_modelPos.y);
            var l_halfLeft = new Vector2(l_modelPos.x - p_model.GetEnemySize().x / 2  -0.15f, l_modelPos.y);
            
            var l_hitOne = Physics2D.CircleCast(l_halfRight, 0.5f, Vector3.right, 1, wallMask);
            var l_hitTwo = Physics2D.CircleCast(l_halfLeft, 0.5f, Vector3.left, 1, wallMask);

            return l_hitOne || l_hitTwo;
        }
        
        
    }
}
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PayerIsLeftOrRight", menuName = "_main/States/Conditions/PayerIsLeftOrRight", order = 0)]
    public class PayerIsLeftOrRight : StateCondition
    {
        [SerializeField] private LayerMask playerMask;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var position = p_model.transform.position;
            
            
            //Physics2D.Linecast(position, position + Vector3.right*20,playerMask)
            //Physics2D.CircleCast(position,0.5f,  Vector2.left, 20f, playerMask)
            //Physics2D.Raycast(position, Vector2.right, 20f, playerMask);
            //Como hacer que el primer hit tenga que ser el player
            return Physics2D.Raycast(position, Vector2.right, 20f, playerMask) ||
                   Physics2D.Raycast(position, Vector2.left, 20f, playerMask);
        }
    }
}
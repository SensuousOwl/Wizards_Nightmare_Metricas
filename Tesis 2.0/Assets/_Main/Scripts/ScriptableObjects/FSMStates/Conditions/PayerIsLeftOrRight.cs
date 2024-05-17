using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PayerIsLeftOrRight", menuName = "_main/States/Conditions/PayerIsLeftOrRight", order = 0)]
    public class PayerIsLeftOrRight : StateCondition
    {
        [SerializeField] private LayerMask playerMask;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var l_position = p_model.transform.position;
            
            
            
            return Physics2D.Raycast(l_position, Vector2.right, 20f, playerMask) ||
                   Physics2D.Raycast(l_position, Vector2.left, 20f, playerMask);
        }
    }
}
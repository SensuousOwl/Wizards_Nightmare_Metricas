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
            return p_model.GetIsTouching();
        }
        
        
    }
}
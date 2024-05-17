using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "HitWallCondition", menuName = "_main/States/Conditions/HitWallCondition", order = 0)]
    public class HitWallCondition : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return p_model.GetIsTouching();
        }
        
        
    }
}
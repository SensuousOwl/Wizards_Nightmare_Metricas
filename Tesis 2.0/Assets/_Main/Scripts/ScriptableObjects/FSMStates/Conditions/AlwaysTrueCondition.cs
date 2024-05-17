using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "AlwaysTrueCondition", menuName = "_main/States/Conditions/AlwaysTrueCondition", order = 0)]
    public class AlwaysTrueCondition : StateCondition
    {
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return true;
        }
    }
}
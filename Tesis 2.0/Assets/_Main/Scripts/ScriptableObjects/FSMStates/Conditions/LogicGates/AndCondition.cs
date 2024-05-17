using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions.LogicGates
{
    [CreateAssetMenu(fileName = "AndCondition", menuName = "_main/States/Conditions/Logic/AND")]
    public class AndCondition : StateCondition
    {
        [SerializeField] private StateCondition conditionOne;
        [SerializeField] private StateCondition conditionTwo;
        
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return conditionOne.CompleteCondition(p_model) && conditionTwo.CompleteCondition(p_model);
        }
    }
}
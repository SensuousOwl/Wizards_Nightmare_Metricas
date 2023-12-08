using _Main.Scripts.Enemies;
using UnityEngine;

namespace _Main.Scripts.FSM.Base.LogicGates
{
    [CreateAssetMenu(fileName = "AndCondition", menuName = "_main/States/Conditions/Logic/OR")]
    public class OrCondition : StateCondition
    {
        [SerializeField] private StateCondition conditionOne;
        [SerializeField] private StateCondition conditionTwo;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return conditionOne.CompleteCondition(p_model) || conditionTwo.CompleteCondition(p_model);
        }
    }
}
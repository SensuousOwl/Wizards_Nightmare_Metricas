using _Main.Scripts.Enemies;
using UnityEngine;

namespace _Main.Scripts.FSM.Base.LogicGates
{
    [CreateAssetMenu(fileName = "AndCondition", menuName = "_main/States/Conditions/Logic/NEGATE")]
    public class NegateCondition : StateCondition
    {
        [SerializeField] private StateCondition condition;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return !condition.CompleteCondition(p_model);
        }
    }
}
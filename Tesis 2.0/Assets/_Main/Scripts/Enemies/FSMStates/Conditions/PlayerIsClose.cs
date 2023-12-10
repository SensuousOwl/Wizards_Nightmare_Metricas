using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PlayerIsClose", menuName = "_main/States/Conditions/PlayerIsClose", order = 0)]
    public class PlayerIsClose : StateCondition
    {
        [SerializeField] private float range;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            Debug.Log($"Distance {(p_model.GetTargetTransform().position - p_model.transform.position).magnitude}");
            return (p_model.GetTargetTransform().position - p_model.transform.position).magnitude <= range;
        }
    }
}
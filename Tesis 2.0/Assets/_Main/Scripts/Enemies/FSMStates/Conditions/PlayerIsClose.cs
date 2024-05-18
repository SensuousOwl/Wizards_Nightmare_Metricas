using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PlayerIsClose", menuName = "_main/States/Conditions/PlayerIsClose", order = 0)]
    public class PlayerIsClose : StateCondition
    {
        [SerializeField] private float range;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return (LevelManager.Instance.PlayerModel.transform.position - p_model.transform.position).magnitude <= range;
        }
    }
}
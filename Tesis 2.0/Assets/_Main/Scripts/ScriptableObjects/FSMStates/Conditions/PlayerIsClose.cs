using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "PlayerIsClose", menuName = "_main/States/Conditions/PlayerIsClose", order = 0)]
    public class PlayerIsClose : StateCondition
    {
        [SerializeField] private float range;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            return (PlayerModel.Local.transform.position - p_model.transform.position).magnitude <= range;
        }
    }
}
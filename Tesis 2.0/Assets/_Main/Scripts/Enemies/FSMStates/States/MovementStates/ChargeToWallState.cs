using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "ChargeToWallState", menuName = "_main/States/Executions/Movement/ChargeToWallState", order = 0)]
    public class ChargeToWallState : MyState
    {
        private class data
        {
            public Vector3 dir;
            public float prepTime;
        }
        [SerializeField] private float prepTime;
        [SerializeField] private string chargeAnimatorBool;
        private Dictionary<EnemyModel, data> models = new Dictionary<EnemyModel, data>();
        
        public override void EnterState(EnemyModel p_model)
        {
            var player = LevelManager.Instance.PlayerModel;
            var l_playerPos = Vector3.zero;
            
            if (player != default)
            {
                l_playerPos = player.transform.position;
            }
            
            
            var l_data = new data();
            if (l_playerPos.x > p_model.transform.position.x)
            {
                l_data.dir = Vector3.right;
            }
            else
            {
                l_data.dir = Vector3.left;
            }


            l_data.prepTime = Time.time + prepTime;
            models[p_model] = l_data;
            p_model.View.SetAnimatorBool(chargeAnimatorBool, true);
            p_model.SetIsRunning(true);
        }
        
        public override void ExecuteState(EnemyModel p_model)
        {
            if(models[p_model].prepTime >= Time.time)
                return;
            
            p_model.MoveWithAcceleration(models[p_model].dir, models[p_model].prepTime);
        }

        public override void ExitState(EnemyModel p_model)
        {
            p_model.View.SetAnimatorBool(chargeAnimatorBool, false);
            models[p_model] = default;
            p_model.SetIsRunning(false);
            p_model.SetIsTouching(false);
            models.Remove(p_model);
        }
    }
}
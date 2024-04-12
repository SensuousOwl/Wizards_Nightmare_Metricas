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
        
        private Dictionary<EnemyModel, data> models = new Dictionary<EnemyModel, data>();
        
        public override void EnterState(EnemyModel p_model)
        {
            var l_playerPos = LevelManager.Instance.PlayerModel.transform.position;
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
        }
        
        public override void ExecuteState(EnemyModel p_model)
        {
            if(models[p_model].prepTime >= Time.time)
                return;
            
            p_model.MoveWithAcceleration(models[p_model].dir, models[p_model].prepTime);
        }

        public override void ExitState(EnemyModel p_model)
        {
            models[p_model] = default;
            models.Remove(p_model);
        }
    }
}
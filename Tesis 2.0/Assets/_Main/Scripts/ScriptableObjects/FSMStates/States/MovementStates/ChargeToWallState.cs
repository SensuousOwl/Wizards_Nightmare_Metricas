using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "ChargeToWallState", menuName = "_main/States/Executions/Movement/ChargeToWallState", order = 0)]
    public class ChargeToWallState : MyState
    {
        private class Data
        {
            public Vector3 Dir;
            public float PrepTime;
        }
        [SerializeField] private float prepTime;
        [SerializeField] private string chargeAnimatorBool;
        private Dictionary<EnemyModel, Data> m_models = new Dictionary<EnemyModel, Data>();
        
        public override void EnterState(EnemyModel p_model)
        {
            var l_player = PlayerModel.Local;
            var l_playerPos = Vector3.zero;
            
            if (l_player != default)
            {
                l_playerPos = l_player.transform.position;
            }
            
            
            var l_data = new Data();
            if (l_playerPos.x > p_model.transform.position.x)
            {
                l_data.Dir = Vector3.right;
            }
            else
            {
                l_data.Dir = Vector3.left;
            }


            l_data.PrepTime = Time.time + prepTime;
            m_models[p_model] = l_data;
            p_model.View.SetAnimatorBool(chargeAnimatorBool, true);
            p_model.SetIsRunning(true);
        }
        
        public override void ExecuteState(EnemyModel p_model)
        {
            if(m_models[p_model].PrepTime >= Time.time)
                return;
            
            p_model.MoveWithAcceleration(m_models[p_model].Dir, m_models[p_model].PrepTime);
        }

        public override void ExitState(EnemyModel p_model)
        {
            p_model.View.SetAnimatorBool(chargeAnimatorBool, false);
            m_models[p_model] = default;
            p_model.SetIsRunning(false);
            p_model.SetIsTouching(false);
            m_models.Remove(p_model);
        }
    }
}
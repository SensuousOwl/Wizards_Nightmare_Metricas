using System;
using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using _Main.Scripts.Steering_Behaviours;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace _Main.Scripts.Enemies.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "InterceptPlayer", menuName = "_main/States/Executions/Movement/InterceptPlayer", order = 0)]
    public class InterceptPlayer : MyState
    {
        [SerializeField] private float interceptTime = 1f;
        [SerializeField] private float minAccValue = 1f;
        [SerializeField] private float maxAccValue = 5f;

        private Dictionary<EnemyModel, PlayerModel> m_datas = new Dictionary<EnemyModel, PlayerModel>();

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        
        public override void EnterState(EnemyModel p_model)
        {
            m_datas[p_model] = LevelManager.Instance.PlayerModel;
            
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var wantedDir = MySteeringBehaviors.GetInterceptDir(p_model.transform.position, m_datas[p_model].transform.position,
                m_datas[p_model].CurrDir, StatsService.GetStatById(StatsId.MovementSpeed), interceptTime);

            var accMult = CalculateMult(p_model.transform.position, m_datas[p_model].transform.position, 
                minAccValue, maxAccValue);
            
            p_model.MoveWithAcceleration(wantedDir, accMult);
        }

        private float CalculateMult(Vector3 p_enemyPos, Vector3 p_targetPos, float minValue, float maxValue)
        {
            //El objetivo de este metodo es que la entidad se mueva mas rapidamente cuanto mas lejos este del objetivo
            // muy util para des-acelerar si pasa de largo
            var distance = Vector3.Distance(p_enemyPos, p_targetPos);
            
            var result = (float)Math.Pow(distance, 2);
        
        
            return Math.Clamp(result, minValue, maxValue);
        }
        public override void ExitState(EnemyModel p_model)
        {
            m_datas[p_model] = default;
        }
    }
}
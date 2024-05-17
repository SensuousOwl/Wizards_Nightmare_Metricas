using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.MovementStates
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
            m_datas[p_model] = PlayerModel.Local;
            
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var l_wantedDir = MySteeringBehaviors.GetInterceptDir(p_model.transform.position, m_datas[p_model].transform.position,
                m_datas[p_model].CurrDir, StatsService.GetStatById(StatsId.MovementSpeed), interceptTime);

            var l_accMult = CalculateMult(p_model.transform.position, m_datas[p_model].transform.position, 
                minAccValue, maxAccValue);
            
            p_model.MoveWithAcceleration(l_wantedDir, l_accMult);
        }

        private float CalculateMult(Vector3 p_enemyPos, Vector3 p_targetPos, float p_minValue, float p_maxValue)
        {
            //El objetivo de este metodo es que la entidad se mueva mas rapidamente cuanto mas lejos este del objetivo
            // muy util para des-acelerar si pasa de largo
            var l_distance = Vector3.Distance(p_enemyPos, p_targetPos);
            
            var l_result = (float)Math.Pow(l_distance, 2);
        
        
            return Math.Clamp(l_result, p_minValue, p_maxValue);
        }
        public override void ExitState(EnemyModel p_model)
        {
            m_datas[p_model] = default;
        }
    }
}
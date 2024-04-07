using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Steering_Behaviours;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "InterceptPlayer", menuName = "_main/States/Executions/InterceptPlayer", order = 0)]
    public class InterceptPlayer : MyState
    {
        [SerializeField] private float interceptTime = 1f;
        

        private Dictionary<EnemyModel, PlayerModel> m_datas = new Dictionary<EnemyModel, PlayerModel>();
        public override void EnterState(EnemyModel p_model)
        {
            m_datas[p_model] = LevelManager.Instance.PlayerModel;
            
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var wantedDir = MySteeringBehaviors.GetInterceptDir(p_model.transform.position, m_datas[p_model].transform.position,
                m_datas[p_model].CurrDir, m_datas[p_model].StatsController.GetStatById(StatsId.MovementSpeed), interceptTime);
            
            p_model.Move(wantedDir);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_datas[p_model] = default;
        }
    }
}
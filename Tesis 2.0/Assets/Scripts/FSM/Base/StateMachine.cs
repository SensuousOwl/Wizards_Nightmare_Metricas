using System;
using System.Collections.Generic;
using System.Linq;
using _main.Scripts.Services;
using _main.Scripts.Services.MicroServices.EventsServices;
using Enemies;

namespace FSM.Base
{
    public sealed class StateMachine
    {
        private StateData m_currentState;
        private int m_currentStateConditionsAmount;

        private readonly Dictionary<Type, StateData> m_statesDictionary = new();
        private readonly EnemyModel m_model;

        private static IEventService EventService => ServiceLocator.Get<IEventService>();

        public StateMachine(EnemyModel p_model)
        {
            EventService.AddListener<ChangeEnemyStateCustomEventData>(OnChangeStateHandler);
            m_model = p_model;
            var l_enemyStates = p_model.GetData().AllStatesData;
            InitializedStatesCheck(l_enemyStates);
            InitializeStates(l_enemyStates);
            
            m_currentState = l_enemyStates[0];
            m_currentState.MyState.EnterState(m_model);
            m_currentStateConditionsAmount = m_currentState.StateConditions.Length;
        }

        ~StateMachine()
        {
            EventService.RemoveListener<ChangeEnemyStateCustomEventData>(OnChangeStateHandler);
        }

        private void OnChangeStateHandler(ChangeEnemyStateCustomEventData p_data)
        {
            if (p_data.Model != m_model)
                return;
            
            ChangeState(p_data.StateType);
        }

        private void InitializeStates(IReadOnlyList<StateData> p_enemyStatesData)
        {
            var l_statesCount = p_enemyStatesData.Count;

            for (var l_i = 0; l_i < l_statesCount; l_i++)
            {
                var l_type = p_enemyStatesData[l_i].MyState.GetType();

                if (m_statesDictionary.ContainsKey(l_type))
                    continue;

                m_statesDictionary.Add(l_type, p_enemyStatesData[l_i]);
            }
        }

        #region InitializationCheck

        private static void InitializedStatesCheck(IReadOnlyList<StateData> p_enemyStatesData)
        {
            if (p_enemyStatesData.Count < 1)
            {
                throw new Exception($"FSM {p_enemyStatesData} has no states assigned");
            }

            for (var l_i = 0; l_i < p_enemyStatesData.Count; l_i++)
            {
                var l_currState = p_enemyStatesData[l_i];

                if (l_currState == null)
                {
                    throw new Exception($"State in position {l_i} is null");
                }

                if (l_currState.ExitStates.Length != l_currState.StateConditions.Length)
                {
                    throw new Exception($"State {l_currState} doesn't have the same amount of exits and conditions");
                }

                if (l_currState.ExitStates.Any(p_exitState => p_exitState == null))
                {
                    throw new Exception($"State {l_currState} has an invalid exit state");
                }

                if (l_currState.StateConditions.Any(p_condition => p_condition == null))
                {
                    throw new Exception($"State {l_currState} has an invalid exit condition");
                }
            }
        }

        #endregion


        public void RunStateMachine()
        {
            if (m_currentState == default) return;

            for (var l_i = 0; l_i < m_currentStateConditionsAmount; l_i++)
            {
                if (!m_currentState.StateConditions[l_i].CompleteCondition(m_model)) 
                    continue;
                
                ChangeState(m_currentState.ExitStates[l_i].MyState.GetType());
                return;
            }

            m_currentState.MyState.ExecuteState(m_model);
        }

        private void ChangeState(Type p_newStateType)
        {
            if (!m_statesDictionary.TryGetValue(p_newStateType, out var l_newStateData))
                return;
            
            m_currentState.MyState.ExitState(m_model);
            m_currentState = l_newStateData;
            m_currentState.MyState.EnterState(m_model);
            m_currentStateConditionsAmount = m_currentState.StateConditions.Length;
        }

        public StateData GetCurrentState() => m_currentState;

    }
    
    public struct ChangeEnemyStateCustomEventData : ICustomEventData
    {
        public EnemyModel Model { get; }
        public Type StateType { get; }

        public ChangeEnemyStateCustomEventData(EnemyModel p_model, Type p_stateType)
        {
            Model = p_model;
            StateType = p_stateType;
        }
    }
}
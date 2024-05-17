using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "SpecialAttacksPool", menuName = "_main/States/Executions/SpecialAttacksPool", order = 0)]
    public class SpecialAttacksPoolState : MyState
    {
        [SerializeField] private List<MyState> specialAttacksStates;
        [SerializeField] private List<float> specialAttacksChances;


        private readonly Dictionary<EnemyModel, MyState> m_models = new();
        private RouletteWheel<MyState> m_rouletteWheel;
        public override void EnterState(EnemyModel p_model)
        {
            m_rouletteWheel ??= new RouletteWheel<MyState>(specialAttacksStates,specialAttacksChances);
            m_models[p_model] = m_rouletteWheel.RunWithCached();
            p_model.SetIsAttacking(true);
            m_models[p_model].EnterState(p_model);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            m_models[p_model].ExecuteState(p_model);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_models[p_model].ExitState(p_model);

            m_models.Remove(p_model);
        }
    }
}
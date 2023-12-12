using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.FSM.Base;
using Unity.VisualScripting;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "SpecialAttacksPool", menuName = "_main/States/Executions/SpecialAttacksPool", order = 0)]
    public class SpecialAttacksPoolState : MyState
    {
        [SerializeField] private List<MyState> specialAttacksStates;
        [SerializeField] private List<float> specialAttacksChances;


        private Dictionary<EnemyModel, MyState> models = new Dictionary<EnemyModel, MyState>();
        public override void EnterState(EnemyModel p_model)
        {
            RouletteWheel<MyState> l_wheel = new RouletteWheel<MyState>(specialAttacksStates,specialAttacksChances);
            models[p_model] = l_wheel.RunWithCached();
            p_model.SetIsAttacking(true);
            models[p_model].EnterState(p_model);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            models[p_model].ExecuteState(p_model);
        }

        public override void ExitState(EnemyModel p_model)
        {
            models[p_model].ExitState(p_model);

            models.Remove(p_model);
        }
    }
}
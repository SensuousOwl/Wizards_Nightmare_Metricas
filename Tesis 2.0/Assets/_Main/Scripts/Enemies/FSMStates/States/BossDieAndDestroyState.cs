using _Main.Scripts.FSM.Base;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "BossDieAndDestroyState", menuName = "_main/States/Executions/BossDieAndDestroyState", order = 0)]
    public class BossDieAndDestroyState : MyState
    {
        [SerializeField] private int currencyReward;
        private static ICurrencyService CurrencyService => ServiceLocator.Get<ICurrencyService>();
        
        public override void EnterState(EnemyModel p_model)
        {
            
            p_model.TriggerDieEvent();
            CurrencyService.AddGs(currencyReward);
            
            Destroy(p_model.gameObject);
        }
        public override void ExecuteState(EnemyModel p_model)
        {
        }
    }
}
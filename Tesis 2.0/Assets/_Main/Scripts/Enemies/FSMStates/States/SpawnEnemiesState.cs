using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "SpawnEnemies", menuName = "_main/States/Executions/SpawnEnemies", order = 0)]
    public class SpawnEnemiesState : MyState
    {
        [SerializeField] private List<EnemyModel> enemiesToSpawn = new List<EnemyModel>();

        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void EnterState(EnemyModel p_model)
        {
            var currEnemies = enemiesToSpawn;

            foreach (var enemy in currEnemies)
            {
                EventService.DispatchEvent(new SpawnEnemyEventData(p_model.MyRoom, enemy, p_model.transform.position));
            }
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            throw new System.NotImplementedException();
        }
    }
}
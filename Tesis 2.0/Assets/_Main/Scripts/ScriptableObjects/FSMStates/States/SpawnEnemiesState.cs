using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventDatas;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "SpawnEnemies", menuName = "_main/States/Executions/SpawnEnemies", order = 0)]
    public class SpawnEnemiesState : MyState
    {
        [SerializeField] private List<EnemyModel> enemiesToSpawn = new List<EnemyModel>();

        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void EnterState(EnemyModel p_model)
        {
            var l_currEnemies = enemiesToSpawn;

            foreach (var l_enemy in l_currEnemies)
            {

                var l_rndVector = new Vector3(Random.value, Random.value);
                EventService.DispatchEvent(new SpawnEnemyEventData(p_model.GetMyRoom(), l_enemy, p_model.transform.position+l_rndVector));
            }
        }

        public override void ExecuteState(EnemyModel p_model){}
    }
}
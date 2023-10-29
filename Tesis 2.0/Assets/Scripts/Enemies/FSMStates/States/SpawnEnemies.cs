using System.Collections.Generic;
using FSM.Base;
using UnityEngine;

namespace Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "SpawnEnemies", menuName = "_main/States/Executions/SpawnEnemies", order = 0)]
    public class SpawnEnemies : MyState
    {
        [SerializeField] private List<EnemyModel> enemiesToSpawn = new List<EnemyModel>();


        public override void EnterState(EnemyModel p_model)
        {
            var currEnemies = enemiesToSpawn;

            foreach (var enemy in currEnemies)
            {
                Instantiate(enemy, p_model.transform.position, p_model.transform.rotation);
            }
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            throw new System.NotImplementedException();
        }
    }
}
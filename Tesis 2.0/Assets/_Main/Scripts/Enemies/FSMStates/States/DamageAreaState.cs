using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "DamageAreaState", menuName = "_main/States/Executions/DamageAreaState", order = 0)]
    public class DamageAreaState : MyState
    {
        [SerializeField] private float damagePerSec;
        [SerializeField] private float radiusEffect;
        [SerializeField] private LayerMask targetMask;
        //Particles / effects
        
        private Dictionary<EnemyModel, RaycastHit2D[]> m_results = new Dictionary<EnemyModel, RaycastHit2D[]>();

        public override void EnterState(EnemyModel p_model)
        {
            //Summon particles
            m_results[p_model] = new RaycastHit2D[10];
            Debug.Log("ENTRO EN LA NUBE");
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var count = Physics2D.CircleCastNonAlloc(p_model.transform.position, radiusEffect,
                Vector2.zero, m_results[p_model], 0, targetMask);
            
            for (int i = 0; i < count; i++)
            {
                var curr = m_results[p_model][i];
                
                if(!curr.transform.TryGetComponent(out IHealthController healthController))
                    return;
                
                healthController.TakeDamage(damagePerSec * Time.deltaTime);
            }
        }

        public override void ExitState(EnemyModel p_model)
        {
            //stop particles
            m_results[p_model] = default;
            m_results.Remove(p_model);
        }
    }
}
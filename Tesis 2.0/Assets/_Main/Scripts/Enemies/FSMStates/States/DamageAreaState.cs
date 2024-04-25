using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "DamageAreaState", menuName = "_main/States/Executions/DamageAreaState", order = 0)]
    public class DamageAreaState : MyState
    {
        private class data
        {
            public RaycastHit2D[] hitArr;
            public ParticleSystem particleSystem;
        }
        [SerializeField] private float damagePerSec;
        [SerializeField] private float radiusEffect;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private GameObject particleSystem;
        //Particles / effects
        
        private Dictionary<EnemyModel, data> m_results = new Dictionary<EnemyModel, data>();

        public override void EnterState(EnemyModel p_model)
        {
            //Summon particles
            
            m_results[p_model] = new data();
            m_results[p_model].hitArr = new RaycastHit2D[10];

            var particles = Instantiate(particleSystem);
            m_results[p_model].particleSystem = particles.GetComponent<ParticleSystem>();
            
            m_results[p_model].particleSystem.transform.position = p_model.transform.position;
            m_results[p_model].particleSystem.Play();
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var count = Physics2D.CircleCastNonAlloc(p_model.transform.position, radiusEffect,
                Vector2.zero, m_results[p_model].hitArr, 0, targetMask);
            
            for (int i = 0; i < count; i++)
            {
                var curr = m_results[p_model].hitArr[i];
                
                if(!curr.transform.TryGetComponent(out IHealthController healthController))
                    return;
                
                healthController.TakeDamage(damagePerSec * Time.deltaTime);
            }
        }

        public override void ExitState(EnemyModel p_model)
        {
            //stop particles
            m_results[p_model].particleSystem.Stop();
            Destroy(m_results[p_model].particleSystem.gameObject);
            m_results[p_model] = default;
            m_results.Remove(p_model);
        }
    }
}
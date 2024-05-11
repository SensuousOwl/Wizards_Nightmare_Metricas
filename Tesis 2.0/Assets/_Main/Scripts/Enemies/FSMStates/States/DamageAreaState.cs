using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "DamageAreaState", menuName = "_main/States/Executions/DamageAreaState", order = 0)]
    public class DamageAreaState : MyState
    {
        private class Data
        {
            public RaycastHit2D[] HitArr;
            public ParticleSystem ParticleSystem;
        }
        [SerializeField] private float damagePerSec;
        [SerializeField] private float radiusEffect;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private string stateAnimName;
        [SerializeField] private GameObject particleSystem;
        //Particles / effects
        
        private Dictionary<EnemyModel, Data> m_results = new Dictionary<EnemyModel, Data>();

        public override void EnterState(EnemyModel p_model)
        {
            m_results[p_model] = new Data();
            m_results[p_model].HitArr = new RaycastHit2D[10];

            var l_particles = Instantiate(particleSystem);
            m_results[p_model].ParticleSystem = l_particles.GetComponent<ParticleSystem>();
            
            m_results[p_model].ParticleSystem.transform.position = p_model.transform.position - Vector3.forward;
            m_results[p_model].ParticleSystem.Play();
            p_model.View.PlayAnim(stateAnimName);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var l_count = Physics2D.CircleCastNonAlloc(p_model.transform.position, radiusEffect,
                Vector2.zero, m_results[p_model].HitArr, 0, targetMask);
            
            for (int l_i = 0; l_i < l_count; l_i++)
            {
                var l_curr = m_results[p_model].HitArr[l_i];
                
                if(!l_curr.transform.TryGetComponent(out IHealthController l_healthController))
                    return;
                
                if(l_healthController == p_model.HealthController)
                    return;
                
                l_healthController.TakeDamage(damagePerSec * Time.deltaTime);
            }
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_results[p_model].ParticleSystem.Stop();
            m_results[p_model] = default;
            m_results.Remove(p_model);
        }
    }
}
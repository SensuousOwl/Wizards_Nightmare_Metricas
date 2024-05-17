using System.Collections.Generic;
using _Main.Scripts.Entities;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "AttackStillState", menuName = "_main/States/Executions/AttackStillState", order = 0)]
    public class AttackStillState : MyState
    {
        
        
        [SerializeField] private float prepareTime;
        [SerializeField] private Vector2 offsetAttack;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask targetMask;

        private class Data
        {
            public float Timer;
            public RaycastHit2D[] Hits;
        }
        
        private Dictionary<EnemyModel, Data> m_datas = new Dictionary<EnemyModel, Data>();
        public override void EnterState(EnemyModel p_model)
        {
            m_datas[p_model] = new Data();
            m_datas[p_model].Timer = Time.time + prepareTime;
            m_datas[p_model].Hits = new RaycastHit2D[20];
            p_model.View.PlayAttackAnim();
            p_model.SetIsAttacking(true);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            if(m_datas[p_model].Timer < Time.time)
                return;

            var l_hit = Physics2D.CircleCastNonAlloc(p_model.transform.position + (Vector3)offsetAttack, attackRadius,
                Vector2.zero, m_datas[p_model].Hits, 0f, targetMask);

            for (int l_i = 0; l_i < l_hit; l_i++)
            {
                var l_curr = m_datas[p_model].Hits[l_i];

                if (l_curr.transform.TryGetComponent(out IHealthController l_healthController))
                {
                    l_healthController.TakeDamage(p_model.GetData().Damage);
                }
            }
            
            p_model.SetIsAttacking(false);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_datas[p_model] = default;
            m_datas.Remove(p_model);
        }
    }
}
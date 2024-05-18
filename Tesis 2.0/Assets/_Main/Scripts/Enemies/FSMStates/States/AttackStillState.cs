using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "AttackStillState", menuName = "_main/States/Executions/AttackStillState", order = 0)]
    public class AttackStillState : MyState
    {
        
        
        [SerializeField] private float prepareTime;
        [SerializeField] private Vector2 offsetAttack;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask targetMask;

        private class data
        {
            public float timer;
            public RaycastHit2D[] hits;
        }
        
        private Dictionary<EnemyModel, data> m_datas = new Dictionary<EnemyModel, data>();
        public override void EnterState(EnemyModel p_model)
        {
            m_datas[p_model] = new data();
            m_datas[p_model].timer = Time.time + prepareTime;
            m_datas[p_model].hits = new RaycastHit2D[20];
            p_model.View.PlayAttackAnim();
            p_model.SetIsAttacking(true);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            if(m_datas[p_model].timer < Time.time)
                return;

            var hit = Physics2D.CircleCastNonAlloc(p_model.transform.position + (Vector3)offsetAttack, attackRadius,
                Vector2.zero, m_datas[p_model].hits, 0f, targetMask);

            for (int i = 0; i < hit; i++)
            {
                var curr = m_datas[p_model].hits[i];

                if (curr.transform.TryGetComponent(out IHealthController healthController))
                {
                    healthController.TakeDamage(p_model.GetData().Damage);
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
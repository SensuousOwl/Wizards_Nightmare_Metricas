using System;
using Extensions;
using FSM.Base;
using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private GameObject TEST_PLAYER;
        private float m_currHp;

        private void Awake()
        {
            m_currHp = data.MaxHp;
        }

        public Transform GetTargetTransform()
        {
            return TEST_PLAYER.transform;
        }

        public EnemyData GetData() => data;

        public void SetLastTargetLocation(Vector3 p_pos)
        {
            
        }

        public void MoveTowards(Vector3 p_targetPoint)
        {
            //TODO: Agregar sistemas de obs avoidance
            p_targetPoint.Xyo();
            var dir = (p_targetPoint - transform.position).normalized;
            transform.position += dir * (data.MovementSpeed * Time.deltaTime);
            Debug.Log($"Targer : {p_targetPoint}, dir {dir}, movement {dir * (data.MovementSpeed * Time.deltaTime)}");
        }


        public void GetDamage(int damage)
        {
            m_currHp -= damage;

            if (m_currHp <= 0)
                Die();
        }

        private void Die()
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer.Equals(data.TargetMask))
            {
                if(!collision.gameObject.TryGetComponent(out IDamageable damageable))
                    return;
                
                damageable.GetDamage(data.Damage);
            }
        }
    }
}
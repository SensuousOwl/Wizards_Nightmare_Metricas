using System;
using Extensions;
using FSM.Base;
using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel : MonoBehaviour, IHealthController
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private GameObject TEST_PLAYER;

        public int CurrHp => m_currHp;
        private int m_currHp;

        private HealthController m_healthController;
        private void Awake()
        {
            m_currHp = data.MaxHp;
            m_healthController = new HealthController(data.MaxHp);
            m_healthController.OnDie += Die;
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
            m_healthController.TakeDamage(damage);
        }

        public void GetHealth(int health)
        {
            m_healthController.Heal(health);
        }

        public void FullHealth()
        {
            m_healthController.RestoreMaxHealth();
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer.Equals(data.TargetMask))
            {
                if(!collision.gameObject.TryGetComponent(out IHealthController damageable))
                    return;
                
                damageable.GetDamage(data.Damage);
            }
        }
    }
}
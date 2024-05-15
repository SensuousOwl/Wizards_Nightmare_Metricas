using System;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;

namespace _Main.Scripts.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private LayerMask wallMask;
        private LayerMask m_targetLayer;
        private Vector3 m_dir;
        private float m_damage;
        private float m_speed;
        private float m_range;

        public event Action<Bullet> OnDeactivate;
        
        public void Initialize(Vector2 p_newPosition, float p_speed, float p_damage, Vector2 p_dir, float p_range,
            LayerMask p_targetMask)
        {
            transform.position = p_newPosition;
            m_dir = p_dir.normalized;
            m_damage = p_damage;
            m_speed = p_speed;
            m_range = p_range;
            m_targetLayer = p_targetMask;
            gameObject.SetActive(true);
        }


        private void FixedUpdate()
        {
            var l_movement = m_dir * (m_speed * Time.deltaTime);
            transform.position += l_movement;
            m_range -= l_movement.magnitude;

            if (m_range <= 0)
            {
                OnDeactivate?.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (LayerMaskExtensions.Includes(wallMask.value, p_other.gameObject.layer))
            {
                OnDeactivate?.Invoke(this);
                return;
                
            }
            if (!LayerMaskExtensions.Includes(m_targetLayer.value, p_other.gameObject.layer)) 
                return;

            if (p_other.TryGetComponent(out IHealthController l_healthController))
            {
                l_healthController.TakeDamage(m_damage);
            }

            OnDeactivate?.Invoke(this);
        }
    }
}
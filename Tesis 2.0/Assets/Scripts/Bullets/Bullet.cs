using System;
using Interfaces;
using UnityEngine;

namespace Bullets
{
        public class Bullet : MonoBehaviour
        {
                [SerializeField] private MeshRenderer Renderer;
                
                private LayerMask m_targetLayer;
                private Vector3 m_dir;
                private int m_damage;
                private float m_speed;
                private float m_range;
                private Color m_color;
                
                public void Initialize(Vector2 p_pos,float p_speed, int p_damage, Vector2 p_dir,float range, LayerMask targetMask)
                { 
                        transform.position = p_pos;
                        m_dir = p_dir.normalized;
                        m_damage = p_damage;
                        m_speed = p_speed;
                        m_range = range;
                        m_targetLayer = targetMask;
                        m_color = Renderer.material.color;
                }


                private void Update()
                {
                        if (m_dir != default)
                        {
                                var movement = m_dir * (m_speed * Time.deltaTime);
                                transform.position += movement;
                                m_range -= movement.magnitude;
                                m_color.a = m_range;
                        }

                        Renderer.material.color = m_color;

                        if (m_range <= 0)
                        {
                                Destroy(gameObject);
                        }
                }


                private void OnCollisionEnter2D(Collision2D col)
                {
                        if(!col.gameObject.layer.Equals(m_targetLayer))
                                return;

                        if (col.gameObject.TryGetComponent(out IHealthController healthController))
                        {
                                healthController.GetDamage(m_damage);
                        }
                }
        }
}
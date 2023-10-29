using System;
using UnityEngine;

namespace Bullets
{
        public class PlayerBullet : MonoBehaviour
        {
                
                
                private Vector3 m_dir;
                private float m_damage;
                private float m_speed;
                
                
                public void Initialize(float p_speed, float p_damage, Vector2 p_dir)
                { 
                        m_dir = p_dir.normalized;
                        m_damage = p_damage;
                        m_speed = p_speed;
                }


                private void Update()
                {
                        if (m_dir != default)
                        {
                                transform.position += m_dir * (m_speed * Time.deltaTime);
                        }
                }


        }
}
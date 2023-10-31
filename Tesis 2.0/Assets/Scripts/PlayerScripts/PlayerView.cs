using System;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimData animData;
        private Animator m_animator;
        [SerializeField] private SpriteRenderer renderer;

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
        }


        public void UpdateDir(Vector3 p_dir)
        {
            renderer.flipX = p_dir.x < 0;
        }
        public void PlayIdleAnim()
        {
            m_animator.Play(animData.IdleNameAnim);

        }

        public void PlayAttackAnim()
        {
            m_animator.Play(animData.AttackNameAnim);
        }

        public void SetWalkSpeed(float speed)
        {
            m_animator.SetFloat("Speed", speed);
        }
        public void PlayHurtAnim()
        {
            m_animator.Play(animData.HurtNameAnim);
        }

        public void PlayDeadAnim()
        {
            m_animator.Play(animData.DeadNameAnim);
        }
    }
}
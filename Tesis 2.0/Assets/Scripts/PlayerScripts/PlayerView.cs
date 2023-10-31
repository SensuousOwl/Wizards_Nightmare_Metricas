using System;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimData animData;
        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        public void PlayIdleAnim()
        {
            m_animator.Play(animData.IdleNameAnim);

        }

        public void PlayAttackAnim()
        {
            m_animator.Play(animData.AttackNameAnim);
        }

        public void PlayWalkAnim()
        {
            m_animator.Play(animData.WalkNameAnim);
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
using UnityEngine;

namespace _Main.Scripts.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        
        private Animator m_animator;
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private bool isLookingLeft;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
        }


        public void UpdateDir(Vector3 p_dir)
        {
            if(!isLookingLeft)
                renderer.flipX = p_dir.x < 0;
            else
                renderer.flipX = p_dir.x > 0;
        }
        public void PlayIdleAnim()
        {
            m_animator.Play("Idle");

        }

        public void PlayTeleportAnim()
        {
            m_animator.Play("Teleport");
        }
        public void PlayAttackAnim()
        {
            m_animator.Play("Attack");
        }

        public void SetWalkSpeed(float speed)
        {
            m_animator.SetFloat(Speed, speed);
        }
        public void PlayHurtAnim()
        {
            m_animator.Play("Hurt");
        }

        public void PlayDeadAnim()
        {
            m_animator.Play("Death");
        }

        public void SetAnimatorBool(string p_parameterName, bool p_value)
        {
            m_animator.SetBool(p_parameterName, p_value);
        }
        public void PlayAnim(string p_stateName)
        {
            m_animator.Play(p_stateName);
        }
    }
}
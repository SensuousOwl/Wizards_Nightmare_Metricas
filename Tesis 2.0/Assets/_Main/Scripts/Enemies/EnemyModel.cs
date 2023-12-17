using System;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.PlayerScripts;
using UnityEngine;
using LayerMaskExtensions = _Main.Scripts.DevelopmentUtilities.LayerMaskExtensions;

namespace _Main.Scripts.Enemies
{
    [RequireComponent(typeof(HealthController))]
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        private int m_currHp;

        public EnemyView View => m_view;
        private EnemyView m_view;
        private Transform m_target;

        public bool IsAttacking => m_isAttacking;
        private bool m_isAttacking;
        public Vector2 CurrDir => m_dir;

        private Vector2 m_dir;
        public IHealthController HealthController { get; private set; }
        public static event Action<EnemyModel> OnDie;
        
        private void Awake()
        {
            HealthController = GetComponent<HealthController>();
            HealthController.Initialize(data.MaxHp);
            m_view = GetComponent<EnemyView>();
            
            // HealthController.OnDie += Die;
            m_timer = 0;
        }

        public Transform GetTargetTransform()
        {
            m_target ??= FindObjectOfType<PlayerModel>().transform;
            return m_target;
        }

        public EnemyData GetData() => data;

        public void SetLastTargetLocation(Vector3 p_pos)
        {
            
        }

        public void SetIsAttacking(bool b) => m_isAttacking = b;

        private Vector3 target;
        public void MoveTowards(Vector3 p_targetPoint)
        {
            target = p_targetPoint;
            p_targetPoint.Xyo();
            m_dir = (p_targetPoint - transform.position).normalized;
            transform.position += (Vector3)m_dir * (data.MovementSpeed * Time.deltaTime);
            m_view.SetWalkSpeed((m_dir * data.MovementSpeed).magnitude);
            m_view.UpdateDir(m_dir);
        }

        public void Move(Vector3 p_dir)
        {
            transform.position += p_dir * (data.MovementSpeed * Time.deltaTime);
            m_view.UpdateDir(p_dir);
        }
        private void Die()
        {
            OnDie?.Invoke(this);
            ExperienceController.Instance.EnemyModelOnOnDie(this);
            Destroy(gameObject);
        }

        public void TriggerDieEvent() => OnDie?.Invoke(this);
        

        private void OnCollisionStay2D(Collision2D other)
        {
            if (m_timer > Time.time)
                return;
            
            if (!LayerMaskExtensions.Includes(data.TargetMask, other.gameObject.layer)) 
                return;
            
            if(!other.gameObject.TryGetComponent(out IHealthController l_healthController))
                return;
            
            
            l_healthController.TakeDamage(data.Damage);
            m_view.PlayAttackAnim();
            m_timer = Time.time + 1;
        }

        private float m_timer;
        
        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(transform.position, m_dir);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target);
        }

#endif
    }
}

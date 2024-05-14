using System;
using _Main.Scripts.Audio;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Grid;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.RoomsSystem;
using UnityEngine;
using LayerMaskExtensions = _Main.Scripts.DevelopmentUtilities.LayerMaskExtensions;

namespace _Main.Scripts.Enemies
{
    [RequireComponent(typeof(HealthController))]
    [RequireComponent(typeof(SfxAudioPoolPlayer))]
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        private int m_currHp;

        public EnemyView View => m_view;
        private EnemyView m_view;
        private Rigidbody2D m_rb;

        public bool IsAttacking => m_isAttacking;
        private bool m_isAttacking;
        public Vector2 CurrDir => m_dir;

        private Vector2 m_dir;

        private DamageFlash m_damageFlash;
        
        public Room MyRoom { get; private set; }
        public HealthController HealthController { get; private set; }
        public ISfxAudioPlayer SfxAudioPlayer { get; private set; }
        public static event Action<float> OnExperienceDrop;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();

        private void Awake()
        {
            HealthController = GetComponent<HealthController>();
            SfxAudioPlayer = GetComponent<ISfxAudioPlayer>();
            HealthController.Initialize(data.MaxHp);
            m_view = GetComponent<EnemyView>();
            m_rb = GetComponent<Rigidbody2D>();
            m_damageFlash = GetComponentInChildren<DamageFlash>();
            
            HealthController.OnTakeDamage += OnOnTakeDamageHC;
            HealthController.OnDie += OnDieHC;
            m_timer = 0;
        }
        
        public EnemyData GetData() => data;

        public void SetLastTargetLocation(Vector3 p_pos)
        {
        }

        public void SetEnemyRoom(Room p_room) => MyRoom = p_room;

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
            m_dir = p_dir;
            transform.position += p_dir * (data.MovementSpeed * Time.deltaTime);
            m_view.UpdateDir(p_dir);
            m_view.SetWalkSpeed((p_dir * data.MovementSpeed).magnitude);
        }

        public void MoveWithAcceleration(Vector2 p_dir, float p_accMult)
        {
            m_dir = p_dir.normalized;

            var accelerationVector = m_dir * (data.AccelerationRate * p_accMult);

            var velocity = m_rb.velocity;
            velocity += accelerationVector * Time.deltaTime;

            m_rb.velocity = Vector2.ClampMagnitude(velocity, data.TerimnalVelocity);
            m_view.SetWalkSpeed((m_rb.velocity).magnitude);
        }


        public void TriggerDieEvent()
        {
            OnExperienceDrop?.Invoke(data.ExperienceDrop);
            EventService.DispatchEvent(new DieEnemyEventData(MyRoom.Grid.GetNodeFromWorldPoint(transform.position).WorldPos, this));
        }

        private void OnDieHC()
        {
            m_view.PlayDeadAnim();
        }


        private void OnOnTakeDamageHC(float obj)
        {
            m_damageFlash.CallDamageFlash();
        }

        private float m_timer;

        private void OnCollisionStay2D(Collision2D other)
        {
            if (m_timer > Time.time)
                return;

            if (!LayerMaskExtensions.Includes(data.TargetMask, other.gameObject.layer))
                return;

            if (!other.gameObject.TryGetComponent(out IHealthController l_healthController))
                return;

            l_healthController.TakeDamage(data.Damage);
            m_view.PlayAttackAnim();
            m_timer = Time.time + 1;
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + (Vector3)m_dir);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target);
        }

#endif
    }
}
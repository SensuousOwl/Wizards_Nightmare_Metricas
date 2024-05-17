using System;
using _Main.Scripts.Audio;
using _Main.Scripts.DamageFlasher;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventDatas;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Entities.Enemies.MVC
{
    [RequireComponent(typeof(HealthController))]
    [RequireComponent(typeof(SfxAudioPoolPlayer))]
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private new BoxCollider2D collider;

        public Vector2 GetEnemySize() => collider.size;
        private int m_currHp;

        public EnemyView View => m_view;
        private EnemyView m_view;
        private Rigidbody2D m_rb;

        public bool IsAttacking => m_isAttacking;
        private bool m_isAttacking;
        public Vector2 CurrDir => m_dir;

        private Vector2 m_dir;

        private DamageFlash m_damageFlash;

        private Room m_myRoom;
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
            
            HealthController.OnTakeDamage += OnTakeDamageHC;
            HealthController.OnDie += OnDieHC;
            m_timer = 0;
        }
        
        public EnemyData GetData() => data;

        public void SetEnemyRoom(Room p_room)=>m_myRoom = p_room;
        public Room GetMyRoom() => m_myRoom;

        public void SetIsAttacking(bool b) => m_isAttacking = b;

        private Vector3 m_target;

        public void MoveTowards(Vector3 p_targetPoint)
        {
            m_target = p_targetPoint;
            p_targetPoint.Xyo();
            
            var l_position = transform.position;
            m_dir = (p_targetPoint - l_position).normalized;
            l_position += (Vector3)m_dir * (data.MovementSpeed * Time.deltaTime);
            
            transform.position = l_position;
            
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

            var l_accelerationVector = m_dir * (data.AccelerationRate * p_accMult);

            var l_velocity = m_rb.velocity;
            l_velocity += l_accelerationVector * Time.deltaTime;

            m_rb.velocity = Vector2.ClampMagnitude(l_velocity, data.TerminalVelocity);
            m_view.SetWalkSpeed((m_rb.velocity).magnitude);
        }


        public void TriggerDieEvent()
        {
            OnExperienceDrop?.Invoke(data.ExperienceDrop);
            if (data.IsBoss)
            {
                EventService.DispatchEvent(new DieEnemyEventData(transform.position, this));
                
                return;
            }

            var l_node = m_myRoom.Grid.GetNearestWalkableNode(transform.position);
            if (l_node != null)
            {
                EventService.DispatchEvent(new DieEnemyEventData(l_node.WorldPos, this));
                return;
            }
            EventService.DispatchEvent(new DieEnemyEventData(transform.position, this));
        }
        
        private void OnDieHC()
        {
            m_view.PlayDeadAnim();
        }


        private void OnTakeDamageHC(float obj)
        {
            m_damageFlash.CallDamageFlash();
        }

        private float m_timer;

        private void OnCollisionStay2D(Collision2D p_other)
        {
            if (m_timer > Time.time)
                return;

            if (!data.TargetMask.Includes(p_other.gameObject.layer))
                return;

            if (!p_other.gameObject.TryGetComponent(out IHealthController l_healthController))
                return;

            l_healthController.TakeDamage(data.Damage);
            m_view.PlayAttackAnim();
            m_timer = Time.time + 1;
        }

        private bool m_isTouching;
        public void SetIsTouching(bool p_b) => m_isTouching = p_b;
        public bool GetIsTouching() => m_isTouching;
        public void SetIsRunning(bool p_b) => m_isRunning = p_b;
        private bool m_isRunning;
        private void OnCollisionEnter2D(Collision2D p_other)
        {
            if(!m_isRunning)
                return;

            m_isTouching = true;
        }

        private void OnCollisionExit2D(Collision2D p_other)
        {
            if(!m_isRunning)
                return;

            m_isTouching = false;
        }

        public void ApplyForce(Vector2 p_dir, float p_magnitude)
        {
            m_rb.AddForce(p_dir * p_magnitude, ForceMode2D.Impulse);
        }

        public void SetRbSpeed(Vector2 p_speed)
        {
            m_rb.velocity = p_speed;
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + (Vector3)m_dir);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_target);
        }

#endif
    }
}

using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthController))]
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        private Rigidbody2D m_rigidbody;
        private StatsData m_myStats;

        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_dashTimer;
        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        public IHealthController HealthController { get; private set; }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            HealthController = GetComponent<HealthController>();
            HealthController.Initialize(playerData.MaxHp);
        }

        private void Start()
        {
            InitializeStats();

            m_fireRateTimer = 0f;
            m_dashTimer = 0f;
            m_mainCamera = Camera.main;
        }

        private void InitializeStats()
        {
            m_myStats = new StatsData(playerData);
        }

        public void UpdateStats(StatsData p_statsData)
        {
            m_myStats = p_statsData;
        }

        public void Move(Vector3 p_dir)
        {
            var l_newPosition = transform.position + p_dir * (m_myStats.CurrMovementSpeed * Time.deltaTime);
            m_rigidbody.MovePosition(l_newPosition);
        }

        public void Dash(Vector3 p_dir)
        {
            if (m_dashTimer > Time.time)
                return;

            m_rigidbody.AddForce(p_dir * m_myStats.CurrDashTrans, ForceMode2D.Impulse);
            m_dashTimer = m_myStats.CurrDashCooldown + Time.time;
        }

        public void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if (m_fireRateTimer > Time.time)
                return;

            var l_position = transform.position;
            var l_bull = Instantiate(playerData.Bullet, l_position, playerData.Bullet.transform.rotation);
            l_bull.Initialize(m_myStats.CurrProjectileSpeed, m_myStats.CurrDamage,
                (m_crossAirPos - l_position).normalized, m_myStats.CurrRange, playerData.TargetLayer);
            m_fireRateTimer = Time.time + m_myStats.CurrFireRate;
        }

        public void UpdateCrossAir(Vector3 p_pos)
        {
            m_crossAirPos = m_mainCamera.ScreenToWorldPoint(p_pos);
        }

        private void Die()
        {
            Debug.Log($"YOU DIED");
        }
    }
}
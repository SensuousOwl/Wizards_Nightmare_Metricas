using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthController))]
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        private Rigidbody2D m_rigidbody;

        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_dashTimer;
        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        public IHealthController HealthController { get; private set; }
        public StatsController StatsController { get; private set; };

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            HealthController = GetComponent<IHealthController>();
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
            StatsController = new StatsController(playerData);
        }

        public void UpdateStats(StatsController p_statsController)
        {
            StatsController = p_statsController;
        }

        public void Move(Vector3 p_dir)
        {
            var l_newPosition = transform.position + p_dir * (StatsController.GetStatById(StatsId.MovementSpeed) * Time.deltaTime);
            m_rigidbody.MovePosition(l_newPosition);
        }

        public void Dash(Vector3 p_dir)
        {
            if (m_dashTimer > Time.time)
                return;

            m_rigidbody.AddForce(p_dir * StatsController.GetStatById(StatsId.DashTranslation), ForceMode2D.Impulse);
            m_dashTimer = StatsController.GetStatById(StatsId.DashCooldown) + Time.time;
        }

        public void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if (m_fireRateTimer > Time.time)
                return;

            var l_position = transform.position;
            var l_bull = Instantiate(playerData.Bullet, l_position, playerData.Bullet.transform.rotation);
            l_bull.Initialize(StatsController.GetStatById(StatsId.ProjectileSpeed), StatsController.GetStatById(StatsId.Damage),
                (m_crossAirPos - l_position).normalized, StatsController.GetStatById(StatsId.Range), playerData.TargetLayer);
            m_fireRateTimer = Time.time + StatsController.GetStatById(StatsId.FireRate);
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
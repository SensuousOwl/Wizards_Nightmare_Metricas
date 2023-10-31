using System.Text;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerModel : MonoBehaviour, IHealthController
    {
        [SerializeField] private PlayerData playerData;

        public struct StatsData
        {
            public float CurrMovementSpeed;
            public float CurrEnergy;
            public float CurrFireRate;
            public float CurrRange;
            public float CurrCriticalChance;
            public float CurrDashCooldown;
            public float CurrDashTrans;
            public float CurrProjectileSpeed;
            public int CurrDamage;
        }

        private StatsData m_myStats;
        private int m_currMaxHp;
        
        
        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_dashTimer;
        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        private HealthController m_healthController;
        private void Start()
        {
            InitializeStats();

            m_fireRateTimer = 0f;
            m_dashTimer = 0f;
            m_mainCamera = Camera.main;

            m_healthController = new HealthController(m_currMaxHp);
        }

        private void InitializeStats()
        {
            m_myStats = new StatsData();
            
            m_currMaxHp = playerData.MaxHp;

            m_myStats.CurrMovementSpeed = playerData.MovementSpeed;
            m_myStats.CurrEnergy = playerData.Energy;
            m_myStats.CurrFireRate = playerData.FireRate;
            m_myStats.CurrRange = playerData.Range;
            m_myStats.CurrCriticalChance = playerData.CriticalChance;
            m_myStats.CurrDashCooldown = playerData.DashCooldown;
            m_myStats.CurrDashTrans = playerData.DashTranslation;
            m_myStats.CurrProjectileSpeed = playerData.ProjectileSpeed;
            m_myStats.CurrDamage = playerData.Damage;
        }

        public void UpdateStats(StatsData p_statsData)
        {
            m_myStats = p_statsData;
        }
        public void Move(Vector3 p_dir)
        {
            transform.position += p_dir * (m_myStats.CurrMovementSpeed * Time.deltaTime);
        }

        public void Dash(Vector3 p_dir)
        {
            if(m_dashTimer > Time.time)
                return;

            //Todo, hacerlo con impulse en RB
            transform.position += p_dir * m_myStats.CurrDashTrans;
            m_dashTimer = m_myStats.CurrDashCooldown + Time.time;
        }

        public void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if(m_fireRateTimer > Time.time)
                return;

            var bull = Instantiate(playerData.Bullet);
            bull.Initialize(transform.position,m_myStats.CurrProjectileSpeed, m_myStats.CurrDamage,
                (m_crossAirPos - transform.position).normalized, m_myStats.CurrRange, playerData.TargetLayer);
            m_fireRateTimer = Time.time + m_myStats.CurrFireRate;
            
        }

        public void UpdateCrossAir(Vector3 p_pos)
        {
            m_crossAirPos = m_mainCamera.ScreenToWorldPoint(p_pos);
        }

        public void GetDamage(int damage)
        {
            m_healthController.TakeDamage(damage);
        }

        public void GetHealth(int health)
        {
            m_healthController.Heal(health);
        }

        public void FullHealth()
        {
            m_healthController.RestoreMaxHealth();
        }

        public void Die()
        {
            Debug.Log($"YOU DIED");
        }
    }
}
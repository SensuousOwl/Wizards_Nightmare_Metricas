using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;



        public int CurrHp => m_currHp;
        private int m_currHp;
        public int CurrMaxHp => m_currMaxHp;
        private int m_currMaxHp;
        public float CurrMovementSpeed => m_currMovementSpeed;
        private float m_currMovementSpeed;
        public float CurrEnergy => m_currEnergy;
        private float m_currEnergy;
        public float CurrFireRate => m_currFireRate;
        private float m_currFireRate;
        public float CurrRange => m_currRange;
        private float m_currRange;
        public float CurrCriticalChance => m_currCriticalChance;
        private float m_currCriticalChance;
        public float CurrDashCooldown => m_currDashCooldown;
        private float m_currDashCooldown;
        public float CurrDashTrans => m_currDashTrans;
        private float m_currDashTrans;
        
        
        private int m_currDamage;
        public float CurrProjectileSpeed;
        private float m_currCriticalDamageMult;
        private int m_currCoins;
        private int m_currXp;

        private float m_dashTimer;
        private float m_fireRateTimer;
        private Camera m_mainCamera;
        private Vector3 m_crossAirPos;
        private void Start()
        {
            m_currHp = playerData.StartHp;
            m_currMaxHp = playerData.MaxHp;
            m_currMovementSpeed = playerData.MovementSpeed;
            m_currEnergy = playerData.Energy;
            m_currFireRate = playerData.FireRate;
            m_currRange = playerData.Range;
            m_currCriticalChance = playerData.CriticalChance;
            m_currDashCooldown = playerData.DashCooldown;
            m_currDashTrans = playerData.DashTranslation;

            m_fireRateTimer = 0f;
            m_dashTimer = 0f;
            m_mainCamera = Camera.main;
            

        }
        
        public void Move(Vector3 p_dir)
        {
            transform.position += p_dir * (m_currMovementSpeed * Time.deltaTime);
        }

        public void Dash(Vector3 p_dir)
        {
            if(m_dashTimer > Time.time)
                return;

            //Todo, hacerlo con impulse en RB
            transform.position += p_dir * m_currDashTrans;
            m_dashTimer = m_currDashCooldown + Time.time;
        }

        public void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            if(m_fireRateTimer > Time.time)
                return;

            var bull = Instantiate(playerData.PlayerBullet);
            bull.Initialize(CurrProjectileSpeed, m_currDamage,m_crossAirPos - transform.position);
            m_fireRateTimer = Time.time + m_currFireRate;
            
            Debug.Log(m_crossAirPos + " " + (m_crossAirPos - transform.position));
        }

        public void UpdateCrossAir(Vector3 p_pos)
        {
            m_crossAirPos = m_mainCamera.ScreenToWorldPoint(p_pos);
        }
        
    }
}
using System.Text;
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

            m_dashTimer = 0f;
        }
        
        public void Move(Vector3 p_dir)
        {
            transform.position += p_dir * (m_currMovementSpeed * Time.deltaTime);
        }

        public void Dash(Vector3 p_dir)
        {
            if(m_dashTimer > Time.time)
                return;

            transform.position += p_dir * m_currDashTrans;
            m_dashTimer = m_currDashCooldown + Time.time;
        }

        public void Shoot()
        {
            //Check for the rate fire to be > 0f before shooting the next bullet
            
        }

        public void UpdateCrossAir(Vector2 p_pos)
        {
        }
        
    }
}
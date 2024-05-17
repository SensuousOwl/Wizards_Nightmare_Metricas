using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Entities.Enemies
{
    public class BossHealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Slider healthBarFillObj;
        [SerializeField] private Slider easeHealthBarFillObj;
        [SerializeField] private float easeSpeed = 0.005f;

        
        private int m_subscribersAmount;
        private float m_maxHp;
        private float m_currHp;
        private float m_currHpPercentage;
        private bool m_updateBar;

        private void Start()
        {
            healthBar.SetActive(false);
        }

        private HealthController m_healthController;
        public void Subscribe(HealthController controller)
        {
            m_healthController = controller;
            healthBar.SetActive(true);
            m_subscribersAmount++;
            UpdateBarData(controller.GetMaxHealth());
            m_healthController.OnTakeDamage += TakeDamage;
            m_healthController.OnDie += UnSubscribe;
        }

        private void UnSubscribe()
        {
            m_subscribersAmount--;
            m_healthController.OnTakeDamage -= TakeDamage;
            m_healthController.OnDie -= UnSubscribe;
            if (m_subscribersAmount <= 0)
            {
                healthBar.SetActive(false);
                ClearData();
            }
            
        }

        private void Update()
        {
            if(!m_updateBar)
                return;
            
            if(easeHealthBarFillObj.value >= healthBarFillObj.value)
            {
                easeHealthBarFillObj.value = Mathf.Lerp(easeHealthBarFillObj.value, m_currHpPercentage, easeSpeed);
            }
            else
            {
                m_updateBar = false;
            }
        }

        private void UpdateBarData(float p_maxHp)
        {
            m_maxHp += p_maxHp;
            m_currHp += p_maxHp;
            
        }

        private void TakeDamage(float p_damage)
        {
            m_currHp -= p_damage;
            UpdateUi();
        }

        private void UpdateUi()
        {
            m_currHpPercentage = (m_currHp / m_maxHp);
            healthBarFillObj.value = m_currHpPercentage;
            m_updateBar = true;
        }
        
        private void ClearData()
        {
            m_maxHp = default;
            m_currHp = default;
            m_currHpPercentage = default;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Enemies
{
    public class BossHealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Slider healthBarFillObj;
        [SerializeField] private Slider easeHealthBarFillObj;
        [SerializeField] private float easeSpeed = 0.005f;

        //Todo: Esta fue la primer forma que se me ocurrio
        //Si anda, no lo toco
        private int m_subscribersAmount;
        private float m_maxHp;
        private float m_currHp;
        private float m_currHpPercentage;
        private bool m_updateBar;

        private void Start()
        {
            healthBar.SetActive(false);
        }

        public void Subscribe(HealthController controller)
        {
            healthBar.SetActive(true);
            m_subscribersAmount++;
            UpdateBarData(controller.GetMaxHealth());
            controller.OnTakeDamage += TakeDamage;
        }
        
        public void UnSubscribe(HealthController controller)
        {
            m_subscribersAmount--;
            controller.OnTakeDamage -= TakeDamage;

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
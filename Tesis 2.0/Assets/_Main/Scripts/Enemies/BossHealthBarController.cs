using System;
using _Main.Scripts.PlayerScripts;
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

        private float currHpPercentage;
        private bool updateBar;

        private void Start()
        {
            healthBar.SetActive(false);
        }

        public void Initialize(HealthController controller)
        {
            healthBar.SetActive(true);
            controller.OnChangeHealth += UpdateHpBar;
        }
        
        public void Deactivate(HealthController controller)
        {
            healthBar.SetActive(false);
            controller.OnChangeHealth -= UpdateHpBar;
        }

        private void Update()
        {
            if(!updateBar)
                return;
            
            if(easeHealthBarFillObj.value >= healthBarFillObj.value)
            {
                easeHealthBarFillObj.value = Mathf.Lerp(easeHealthBarFillObj.value, currHpPercentage, easeSpeed);
            }
            else
            {
                updateBar = false;
            }
        }

        public void UpdateHpBar(float p_maxHp, float p_currHp)
        {
            currHpPercentage = (p_currHp / p_maxHp);
            healthBarFillObj.value = currHpPercentage;
            updateBar = true;
        }
        
    }
}
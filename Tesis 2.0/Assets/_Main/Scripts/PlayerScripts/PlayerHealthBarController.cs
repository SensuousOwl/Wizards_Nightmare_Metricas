using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.PlayerScripts
{
    public class PlayerHealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Slider healthBarFillObj;
        
        private RectTransform healthBarRectTrans;
        private HealthController m_healthController;
        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForSeconds(1);
            
            
            m_healthController = FindObjectOfType<PlayerModel>().HealthController;
            
            m_healthController.OnChangeHealth += UpdateHpBar;
            m_healthController.OnChangeMaxHealth += UpdateMaxHpBar;
            
            healthBarRectTrans = healthBar.GetComponent<RectTransform>();
        }

        public void UpdateHpBar(float maxHp, float currHp)
        {
            var hpPercentaje = (currHp / maxHp);
            healthBarFillObj.value = hpPercentaje;
            
        }
        
        public void UpdateMaxHpBar(float prevMaxHp, float currMaxHp)
        {
            var diff = (currMaxHp /prevMaxHp);

            healthBarRectTrans.sizeDelta =
                new Vector2(healthBarRectTrans.rect.width * diff, healthBarRectTrans.rect.height);
        }
        
    }
}
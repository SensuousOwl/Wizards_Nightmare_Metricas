using System.Collections;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.PlayerScripts
{
    public class PlayerHealthBarController : MonoBehaviour
    {
        [SerializeField] private RectTransform healthBarRectTrans;
        [SerializeField] private Slider healthBarFillObj;
        
        
        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();

        private float m_previuosMaxHp;
        private HealthController m_healthController;
        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForSeconds(1);

            m_healthController = PlayerModel.Local.HealthController;
            m_previuosMaxHp = m_healthController.GetMaxHealth();
            
            StatsService.OnChangeStatValue += OnChangeMaxHealth;
            m_healthController.OnChangeHealth += UpdateHpBar;
            OnChangeMaxHealth(StatsId.MaxHealth, StatsService.GetBaseStatById(StatsId.MaxHealth));
            UpdateHpBar(m_previuosMaxHp, m_healthController.GetCurrentHealth());
        }

        private void OnChangeMaxHealth(StatsId p_stat, float p_currValue)
        {
            if(p_stat!=StatsId.MaxHealth)
                return;
            
            var l_diff = (p_currValue /m_previuosMaxHp);
            var l_hpPercentaje = (m_healthController.GetCurrentHealth() / p_currValue);
            
            var l_rect = healthBarRectTrans.rect;
            
            healthBarRectTrans.sizeDelta = new Vector2(l_rect.width * l_diff, l_rect.height);
            healthBarFillObj.value = l_hpPercentaje;

            m_previuosMaxHp = p_currValue;
        }

        private void OnDestroy()
        {
            StatsService.OnChangeStatValue -= OnChangeMaxHealth;
            m_healthController.OnChangeHealth -= UpdateHpBar;
        }

        public void UpdateHpBar(float p_maxHp, float p_currHp)
        {
            var l_hpPercentaje = (p_currHp / p_maxHp);
            healthBarFillObj.value = l_hpPercentaje;
            
        }
        
        
    }
}
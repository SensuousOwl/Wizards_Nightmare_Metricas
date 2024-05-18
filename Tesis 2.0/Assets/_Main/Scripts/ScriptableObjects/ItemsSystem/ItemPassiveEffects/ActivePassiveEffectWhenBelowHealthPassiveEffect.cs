using System.Collections.Generic;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/ActivePassiveEffectWhenBelowHealth")]
    public class ActivePassiveEffectWhenBelowHealthPassiveEffect : ItemPassiveEffect
    {
        [SerializeField, Range(1, 100)] private float valueBelowHealthPercentage;
        [SerializeField] private List<ItemPassiveEffect> passiveEffects;

        private bool m_isActive;
        
        public override void Activate()
        {
            PlayerModel.Local.HealthController.OnChangeHealth += HealthControllerOnOnChangeHealth;
        }
        
        private void HealthControllerOnOnChangeHealth(float p_maxHealth, float p_currentHealth)
        {
            var l_value = (p_currentHealth / p_maxHealth) * 100;

            if (l_value <= valueBelowHealthPercentage)
            {
                m_isActive = true;
                foreach (var l_passiveEffect in passiveEffects)
                {
                    l_passiveEffect.Activate();
                }
            }
            else if (m_isActive)
            {
                m_isActive = false;
                foreach (var l_passiveEffect in passiveEffects)
                {
                    l_passiveEffect.Deactivate();
                }
            }
        }

        public override void Deactivate()
        {
            PlayerModel.Local.HealthController.OnChangeHealth -= HealthControllerOnOnChangeHealth;
            
            if (!m_isActive) 
                return;
            
            foreach (var l_passiveEffect in passiveEffects)
            {
                l_passiveEffect.Deactivate();
            }
        }
    }
}
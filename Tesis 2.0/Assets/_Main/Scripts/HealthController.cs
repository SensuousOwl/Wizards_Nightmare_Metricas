using System;
using _Main.Scripts.Attributes;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts
{
    public class HealthController : MonoBehaviour, IHealthController
    {
        [SerializeField] private float maxHealth;
        [ReadOnlyInspector, SerializeField] private float currentHealth;

        public event Action OnDie;
        public event Action<float, float> OnChangeHealth;
        public event Action<float, float, float> OnChangeMaxHealth;
        public event Action<float> OnTakeDamage;

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();

        private bool m_isPlayer;

        public void Initialize(float p_maxHealth)
        {
            maxHealth = p_maxHealth;
            currentHealth = maxHealth;
        }
        
        public void Initialize()
        {
            maxHealth = StatsService.GetStatById(StatsId.MaxHealth);
            currentHealth = maxHealth;
            m_isPlayer = true;
        }

        public void ChangeMaxHealth(float p_newValue)
        {
            if (m_isPlayer)
                return;
            OnChangeMaxHealth?.Invoke(maxHealth, p_newValue, currentHealth);
            maxHealth = p_newValue;
        }
    
        public void AddMaxHealth(float p_newValue)
        {
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth += p_newValue, currentHealth);
            maxHealth += p_newValue;
        }
        public void RemoveMaxHealth(float p_newValue)
        {
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth -= p_newValue, currentHealth);
            maxHealth -= p_newValue;
        }

        public void TakeDamage(float p_damage)
        {
            currentHealth -= p_damage;

            OnTakeDamage?.Invoke(p_damage);
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
            
            if (currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float p_healAmount)
        {
            if (currentHealth >= maxHealth)
                return;
            
            currentHealth += p_healAmount;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public void ClampCurrentHealth()
        {
            if (!(currentHealth >= maxHealth)) 
                return;
            
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public void RestoreMaxHealth()
        {
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
    }
}
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

        private bool m_isInvulnerable;

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

        public void SetInvulnerability(bool p_newValue) => m_isInvulnerable = p_newValue;

        public void ChangeMaxHealth(float p_newValue)
        {
            CheckMaxHealth();
            if (m_isPlayer)
            {
                OnChangeMaxHealth?.Invoke(maxHealth, p_newValue, currentHealth);
                StatsService.SetUpgradeStat(StatsId.MaxHealth, p_newValue);
                return;
            }
            OnChangeMaxHealth?.Invoke(maxHealth, p_newValue, currentHealth);
            maxHealth = p_newValue;
        }
    
        public void AddMaxHealth(float p_value)
        {
            CheckMaxHealth();
            if (m_isPlayer)
            {
                OnChangeMaxHealth?.Invoke(maxHealth, maxHealth -= p_value, currentHealth);
                StatsService.AddUpgradeStat(StatsId.MaxHealth, p_value);
                return;
            }
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth += p_value, currentHealth);
            maxHealth += p_value;
        }
        public void SubtractMaxHealth(float p_value)
        {
            CheckMaxHealth();
            if (m_isPlayer)
            {
                OnChangeMaxHealth?.Invoke(maxHealth, maxHealth -= p_value, currentHealth);
                StatsService.SubtractUpgradeStat(StatsId.MaxHealth, p_value);
                return;
            }
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth -= p_value, currentHealth);
            maxHealth -= p_value;
        }

        public void TakeDamage(float p_damage)
        {
            CheckMaxHealth();
            if (m_isInvulnerable)
                return;
            
            currentHealth -= p_damage;

            OnTakeDamage?.Invoke(p_damage);
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
            
            if (currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float p_healAmount)
        {
            CheckMaxHealth();
            if (currentHealth >= maxHealth)
                return;
            
            currentHealth += p_healAmount;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public void ClampCurrentHealth()
        {
            CheckMaxHealth();
            if (!(currentHealth >= maxHealth)) 
                return;
            
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public void RestoreMaxHealth()
        {
            CheckMaxHealth();
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(maxHealth, currentHealth);
        }

        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth()
        { 
            CheckMaxHealth();
            return maxHealth;
        }

        private void CheckMaxHealth()
        {
            if (!m_isPlayer)
                return;

            if (Mathf.Approximately(maxHealth, StatsService.GetStatById(StatsId.MaxHealth)))
                maxHealth = StatsService.GetStatById(StatsId.MaxHealth);
        }
    }
}
using System;
using _Main.Scripts.Attributes;
using UnityEngine;

namespace _Main.Scripts
{
    public class HealthController : MonoBehaviour, IHealthController
    {
        [SerializeField] private float maxHealth;
        [ReadOnlyInspector, SerializeField] private float currentHealth;

        public event Action OnDie;
        public event Action<float, float> OnChangeHealth;
        public event Action<float, float> OnChangeMaxHealth;
        public event Action<float> OnTakeDamage;

        public void Initialize(float p_maxHealth)
        {
            maxHealth = p_maxHealth;
            currentHealth = maxHealth;
        }

        public void ChangeMaxHealth(float p_newValue)
        {
            OnChangeMaxHealth?.Invoke(maxHealth, p_newValue);
            maxHealth = p_newValue;
        }
    
        public void AddMaxHealth(float p_newValue)
        {
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth += p_newValue);
            maxHealth += p_newValue;
        }
        public void RemoveMaxHealth(float p_newValue)
        {
            OnChangeMaxHealth?.Invoke(maxHealth, maxHealth -= p_newValue);
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
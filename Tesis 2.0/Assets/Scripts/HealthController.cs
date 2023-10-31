using System;
using UnityEngine;

public interface IHealthController
{
    public event Action OnDie;
    public event Action<float, float> OnChangeHealth;
    public event Action<float> OnTakeDamage;
    public void Initialize(float p_maxHealth);
    public void ChangeMaxHealth(float p_newValue);
    public void AddMaxHealth(float p_newValue);
    public void RemoveMaxHealth(float p_newValue);
    public void TakeDamage(float p_damage);
    public void Heal(float p_healAmount);
    public void ClampCurrentHealth();
    public void RestoreMaxHealth();
    public float GetCurrentHealth();
}

public class HealthController : MonoBehaviour, IHealthController
{
    [SerializeField] private float maxHealth;
    private float m_currentHealth;

    public event Action OnDie;
    public event Action<float, float> OnChangeHealth;
    public event Action<float> OnTakeDamage;

    public void Initialize(float p_maxHealth)
    {
        maxHealth = p_maxHealth;
        m_currentHealth = maxHealth;
    }

    public void ChangeMaxHealth(float p_newValue)
    {
        maxHealth = p_newValue;
    }
    
    public void AddMaxHealth(float p_newValue)
    {
        maxHealth += p_newValue;
    }
    public void RemoveMaxHealth(float p_newValue)
    {
        maxHealth += p_newValue;
    }

    public void TakeDamage(float p_damage)
    {
        m_currentHealth -= p_damage;

        OnTakeDamage?.Invoke(p_damage);
        OnChangeHealth?.Invoke(maxHealth, m_currentHealth);
            
        if (m_currentHealth <= 0)
            OnDie?.Invoke();
    }

    public void Heal(float p_healAmount)
    {
        if (m_currentHealth >= maxHealth)
            return;
            
        m_currentHealth += p_healAmount;

        if (m_currentHealth > maxHealth)
            m_currentHealth = maxHealth;
            
        OnChangeHealth?.Invoke(maxHealth, m_currentHealth);
    }

    public void ClampCurrentHealth()
    {
        if (!(m_currentHealth >= maxHealth)) 
            return;
            
        m_currentHealth = maxHealth;
        OnChangeHealth?.Invoke(maxHealth, m_currentHealth);
    }

    public void RestoreMaxHealth()
    {
        m_currentHealth = maxHealth;
        OnChangeHealth?.Invoke(maxHealth, m_currentHealth);
    }

    public float GetCurrentHealth() => m_currentHealth;
}
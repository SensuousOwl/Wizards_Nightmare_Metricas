using System;

public class HealthController
{
    private readonly float m_maxHealth;
    private float m_currentHealth;

    public event Action OnDie;
    public event Action<float, float> OnChangeHealth;
    public event Action<float> OnTakeDamage;
        
    public HealthController(float p_maxHealth)
    {
        m_maxHealth = p_maxHealth;
        m_currentHealth = m_maxHealth;
    }

    public void TakeDamage(float p_damage)
    {
        m_currentHealth -= p_damage;

        OnTakeDamage?.Invoke(p_damage);
        OnChangeHealth?.Invoke(m_maxHealth, m_currentHealth);
            
        if (m_currentHealth <= 0)
            OnDie?.Invoke();
    }

    public void Heal(float p_healAmount)
    {
        if (m_currentHealth >= m_maxHealth)
            return;
            
        m_currentHealth += p_healAmount;

        if (m_currentHealth > m_maxHealth)
            m_currentHealth = m_maxHealth;
            
        OnChangeHealth?.Invoke(m_maxHealth, m_currentHealth);
    }

    public void ClampCurrentHealth()
    {
        if (!(m_currentHealth >= m_maxHealth)) 
            return;
            
        m_currentHealth = m_maxHealth;
        OnChangeHealth?.Invoke(m_maxHealth, m_currentHealth);
    }

    public void RestoreMaxHealth()
    {
        m_currentHealth = m_maxHealth;
        OnChangeHealth?.Invoke(m_maxHealth, m_currentHealth);
    }

    public float GetCurrentHealth() => m_currentHealth;
}
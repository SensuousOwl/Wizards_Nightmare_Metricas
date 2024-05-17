using System;

namespace _Main.Scripts.Entities
{
    public interface IHealthController
    {
        public event Action OnDie;
        public event Action<float, float> OnChangeHealth;
        public event Action<float, float, float> OnChangeMaxHealth;
        public event Action<float> OnTakeDamage;
        public void Initialize(float p_maxHealth);
        public void ChangeMaxHealth(float p_newValue);
        public void AddMaxHealth(float p_value);
        public void SubtractMaxHealth(float p_value);
        public void TakeDamage(float p_damage);
        public void Heal(float p_healAmount);
        public void ClampCurrentHealth();
        public void RestoreMaxHealth();
        public float GetCurrentHealth();
        public float GetMaxHealth();
    }
}
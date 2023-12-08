namespace _Main.Scripts.Interfaces
{
    public interface IHealthController
    {
        void GetDamage(int damage);
        void GetHealth(int health);
        void FullHealth();
        void Die();
    }
}
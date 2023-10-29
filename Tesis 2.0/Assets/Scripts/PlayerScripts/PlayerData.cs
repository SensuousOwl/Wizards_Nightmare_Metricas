using Bullets;
using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_main/Data/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public int StartHp { get; private set; }
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float Energy { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public float DashCooldown { get; private set; }
        [field: SerializeField] public float DashTranslation { get; private set; }
        
        //Esto creo que tendria que estar en otro SO, de las balas
        
        [field: SerializeField] public PlayerBullet PlayerBullet { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        
        [field: SerializeField] public float CriticalDamageMult { get; private set; }
        [field: SerializeField] public float CriticalChance { get; private set; }
        [field: SerializeField] public int StartingCoins { get; private set; }
        [field: SerializeField] public int StartingXp { get; private set; }
    }
}
using _Main.Scripts.Bullets;
using UnityEngine;

namespace _Main.Scripts.PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_main/Data/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [field: Space(10f), Header("Stats")]
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public float FireRange { get; private set; }
        [field: SerializeField] public float DashCooldown { get; private set; }
        [field: SerializeField] public float DashTranslation { get; private set; }
        [field: SerializeField, Min(1f)] public float CriticalDamageMult { get; private set; }
        [field: SerializeField] public float SubtractItemActiveCooldownPercentage { get; private set; }
        [field: SerializeField, Range(0, 100)] public float CriticalChance { get; private set; }
        [field: SerializeField, Range(0, 100)] public float SpawnItemChance { get; private set; }
        [field: SerializeField] public int StartingXp { get; private set; }
        
        [field: Space(10f), Header("BulletsAndDamage")]
        [field: SerializeField] public Bullet Bullet { get; private set; }
        [field: SerializeField] public LayerMask TargetLayer { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        
        [field: Space(10f), Header("Interact")]
        [field: SerializeField] public float InteractRadius { get; private set; }
        [field: SerializeField] public LayerMask InteractLayerMask { get; private set; }
    }
}
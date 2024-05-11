using _Main.Scripts.ItemsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem
{
    [CreateAssetMenu(menuName = "Main/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemActiveEffect ItemActiveEffect { get; private set; }
        [field: SerializeField] public ItemPassiveEffect ItemPassiveEffect { get; private set; }
        [field: SerializeField, Min(1)] public int UseCount { get; private set; } = 1;
        [field: SerializeField] public bool IsItemCooldown { get; private set; }
        [field: SerializeField] public float TimeToCooldownInSeconds { get; private set; }
        
    }
}
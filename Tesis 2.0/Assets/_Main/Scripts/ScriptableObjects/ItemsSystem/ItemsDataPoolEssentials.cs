using _Main.Scripts.ItemsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem
{
    [CreateAssetMenu(menuName = "Main/Items/ItemsDataPool")]
    public class ItemsDataPoolEssentials : ScriptableObject
    {
        [field: SerializeField] public BaseItem ItemPrefab { get; private set; }
        [field: SerializeField] public SerializableDictionary<TypeDropItemPool, ChanceItemPoolConfig> ItemsPoolWeight { get; private set; }
        
        
        [field: SerializeField] public SerializableDictionary<ItemData, float> InstantItems { get; private set; }
        [field: SerializeField] public SerializableDictionary<ItemData, float> CommonItems { get; private set; }
        [field: SerializeField] public SerializableDictionary<ItemData, float> RareItems { get; private set; }
        [field: SerializeField] public SerializableDictionary<ItemData, float> EpicItems { get; private set; }
    }

    [System.Serializable]
    public struct ChanceItemPoolConfig
    {
        [field: SerializeField] public float CommonItemsWeight { get; private set; }
        [field: SerializeField] public float RareItemsWeight { get; private set; }
        [field: SerializeField] public float EpicItemsWeight { get; private set; }
    }
    
    [System.Serializable]
    public enum TypeDropItemPool
    {
        Chest,
        Boss,
        Generic
    }
}
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Main/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public UpgradeEffect UpgradeEffect { get; private set; }
        [field: SerializeField] public float UpgradeValuePercentage { get; private set; }
        [field: SerializeField] public GameObject SpawnObject { get; private set; }
    }
}
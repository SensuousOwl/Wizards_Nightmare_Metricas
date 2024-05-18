using _Main.Scripts.Bullets;
using UnityEngine;

namespace _Main.Scripts.PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "_main/Data/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [field: Space(10f), Header("Stats")]
        [field: SerializeField] public SerializableDictionary<StatsId, float> BaseStatsDictionary { get; private set; }
        [field: SerializeField] public SerializableDictionary<StatsId, float> MaxStatsValueDictionary { get; private set; }
        
        [field: Space(10f), Header("BulletsAndDamage")]
        [field: SerializeField] public Bullet Bullet { get; private set; }
        [field: SerializeField] public LayerMask TargetLayer { get; private set; }
        
        [field: Space(10f), Header("Interact")]
        [field: SerializeField] public float InteractRadius { get; private set; }
        [field: SerializeField] public LayerMask InteractLayerMask { get; private set; }
    }
}
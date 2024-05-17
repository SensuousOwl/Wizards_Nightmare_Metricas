using UnityEngine;

namespace _Main.Scripts.Entities.PlayerScripts.Data
{
    [CreateAssetMenu(fileName = "InputData", menuName = "_main/Data/InputData", order = 0)]
    public class PlayerInputData : ScriptableObject
    {
        [field: SerializeField] public string MovementId { get; private set; }
        [field: SerializeField] public string AimId { get; private set; }
        [field: SerializeField] public string DashId { get; private set; }
        [field: SerializeField] public string ShootId { get; private set; }
        [field: SerializeField] public string InteractId { get; private set; }
        [field: SerializeField] public string UseItemId { get; private set; }
        [field: SerializeField] public string DropItemActive { get; private set; }
        [field: SerializeField] public string DropItemPassive { get; private set; }
    }
}
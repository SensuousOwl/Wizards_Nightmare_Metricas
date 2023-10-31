using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerAnimData", menuName = "_main/Data/PlayerAnimData", order = 1)]
    public class PlayerAnimData : ScriptableObject
    {
        [field: SerializeField] public string IdleNameAnim { get; set; }
        [field: SerializeField]public string AttackNameAnim { get; set; }
        [field: SerializeField]public string WalkNameAnim { get; set; }
        [field: SerializeField]public string HurtNameAnim { get; set; }
        [field: SerializeField]public string DeadNameAnim { get; set; }
    }
}
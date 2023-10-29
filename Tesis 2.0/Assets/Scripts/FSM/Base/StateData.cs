using UnityEngine;

namespace FSM.Base
{
    [CreateAssetMenu(fileName = "StateData", menuName = "_main/Datas/StateData", order = 0)]
    public class StateData : ScriptableObject
    {
        [field: SerializeField] public State State { get; private set; }
        [field: SerializeField] public StateCondition[] StateConditions { get; private set; }
        [field: SerializeField] public StateData[] ExitStates { get; private set; }
        
    }
}

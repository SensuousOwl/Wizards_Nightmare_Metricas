using Enemies;
using UnityEngine;

namespace FSM.Base
{

    public abstract class StateCondition : ScriptableObject
    {
        public abstract bool CompleteCondition(EnemyModel p_model);
    }
}
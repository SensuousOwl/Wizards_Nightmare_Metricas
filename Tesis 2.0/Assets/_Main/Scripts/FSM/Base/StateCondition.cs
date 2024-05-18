using _Main.Scripts.Enemies;
using UnityEngine;

namespace _Main.Scripts.FSM.Base
{

    public abstract class StateCondition : ScriptableObject
    {
        public abstract bool CompleteCondition(EnemyModel p_model);
    }
}
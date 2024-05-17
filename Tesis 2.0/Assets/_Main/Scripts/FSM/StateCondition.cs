using _Main.Scripts.Entities.Enemies.MVC;
using UnityEngine;

namespace _Main.Scripts.FSM
{

    public abstract class StateCondition : ScriptableObject
    {
        public abstract bool CompleteCondition(EnemyModel p_model);
    }
}
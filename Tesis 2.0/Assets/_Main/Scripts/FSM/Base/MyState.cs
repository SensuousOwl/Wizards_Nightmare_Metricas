using _Main.Scripts.Enemies;
using UnityEngine;

namespace _Main.Scripts.FSM.Base
{
    public abstract class MyState : ScriptableObject
    {
        public virtual void EnterState(EnemyModel p_model){}
        public abstract void ExecuteState(EnemyModel p_model);
        public virtual void ExitState(EnemyModel p_model){}
    }
    
}


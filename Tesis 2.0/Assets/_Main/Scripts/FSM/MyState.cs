using _Main.Scripts.Entities.Enemies.MVC;
using UnityEngine;

namespace _Main.Scripts.FSM
{
    public abstract class MyState : ScriptableObject
    {
        public virtual void EnterState(EnemyModel p_model){}
        public abstract void ExecuteState(EnemyModel p_model);
        public virtual void ExitState(EnemyModel p_model){}
    }
    
}


using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem
{
    public abstract class ItemPassiveEffect : ScriptableObject
    {
        public abstract void Activate();
        public abstract void Deactivate();
    }
}
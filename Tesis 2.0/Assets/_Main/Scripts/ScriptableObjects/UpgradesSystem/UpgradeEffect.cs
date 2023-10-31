using PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        [SerializeField] protected float valuePercentage;
        public abstract void ApplyEffect(PlayerModel p_model);
    }
}
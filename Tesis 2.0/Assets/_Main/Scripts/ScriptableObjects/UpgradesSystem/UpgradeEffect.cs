using PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        public abstract void ApplyEffect(PlayerModel p_model, float p_valuePercentage);
    }
}
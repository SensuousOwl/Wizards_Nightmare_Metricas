using System.Collections;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.UpgradesSystem
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        public abstract void ApplyEffect(float p_valuePercentage);
    }
}
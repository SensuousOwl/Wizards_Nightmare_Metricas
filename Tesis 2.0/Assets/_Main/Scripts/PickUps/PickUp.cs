using System;
using UnityEngine;

namespace _Main.Scripts.PickUps
{
    public abstract class PickUp : MonoBehaviour
    {
        public abstract void ApplyEffect();
        public abstract bool ConditionsToApplyEffect(Collider2D other);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ConditionsToApplyEffect(other))
            {
                ApplyEffect();
            }
        }
    }
}
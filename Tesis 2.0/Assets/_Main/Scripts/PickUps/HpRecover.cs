using System;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.PickUps
{
    public class HpRecover : PickUp
    {
        [SerializeField] private float Hp;
        [SerializeField] private LayerMask playerMask;

        private HealthController hpController;
        public override void ApplyEffect()
        {
            hpController.Heal(Hp);
            Destroy(gameObject);
        }

        public override bool ConditionsToApplyEffect(Collider2D other)
        {
            if(!LayerMaskExtensions.Includes(playerMask,other.gameObject.layer))
                return false;

            if (!other.gameObject.TryGetComponent(out PlayerModel l_playerModel))
                return  false;

            hpController = l_playerModel.HealthController;
            
            if(hpController.GetCurrentHealth() == hpController.GetMaxHealth())
                return  false;

            return true;
        }
    }
}
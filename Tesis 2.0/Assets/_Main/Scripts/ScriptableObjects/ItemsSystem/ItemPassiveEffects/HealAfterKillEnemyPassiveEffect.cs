using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/HealAfterKillEnemy")]
    public class HealAfterKillEnemyPassiveEffect : ItemPassiveEffect
    {
        [SerializeField] private float healAmount;
        [SerializeField, Range(1, 100)] private float probability;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void Activate()
        {
            EventService.AddListener<DieEnemyEventData>(Callback);
        }
        
        private void Callback(DieEnemyEventData p_data)
        {
            var l_value = Random.Range(0f, 100f);
            if (l_value <= probability)
                PlayerModel.Local.HealthController.Heal(healAmount);
        }

        public override void Deactivate()
        {
            EventService.RemoveListener<DieEnemyEventData>(Callback);
        }
    }
}
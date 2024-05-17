using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using _Main.Scripts.StaticClass;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/HealAfterCleanRoom")]
    public class HealAfterCleanRoomPassiveEffect : ItemPassiveEffect
    {
        [SerializeField] private float healAmount;
        [SerializeField, Range(1, 100)] private float probability;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void Activate()
        {
            EventService.AddListener(EventsDefinition.CLEAR_ROOM_ID, Callback);
        }
        
        private void Callback()
        {
            var l_value = Random.Range(0f, 100f);
            if (l_value <= probability)
                PlayerModel.Local.HealthController.Heal(healAmount);
        }

        public override void Deactivate()
        {
            EventService.RemoveListener(EventsDefinition.CLEAR_ROOM_ID, Callback);
        }
    }
}
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/KillAllEnemies")]
    public class KillAllEnemiesActiveEffect : ItemActiveEffect
    {
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void UseItem()
        {
            EventService.DispatchEvent(EventsDefinition.KILL_ALL_ENEMIES_ID);
        }
    }
}
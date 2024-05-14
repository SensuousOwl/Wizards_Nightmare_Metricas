using System.Collections;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/ClearRoomAfterXRooms")]
    public class ClearRoomAfterXRooms : ItemPassiveEffect
    {
        [SerializeField] private int roomsCooldown;

        private int m_roomCount;
        private static IEventService EventService => ServiceLocator.Get<IEventService>();
        public override void Activate()
        {
            EventService.AddListener(EventsDefinition.CLEAR_ROOM_ID, OnClearRoom);
            EventService.AddListener(EventsDefinition.ENTER_UNCLEAR_ROOM_ID, KillAllEnemies);
            
        }

        private void KillAllEnemies()
        {
            if (m_roomCount >= roomsCooldown)
            {
                var l_timer = 0f;
                while (l_timer <= 2.5f)
                {
                    l_timer += Time.deltaTime;
                    if (l_timer >= 2f)
                    {
                        EventService.DispatchEvent(EventsDefinition.KILL_ALL_ENEMIES_ID);
                        m_roomCount = 0;
                        break;
                    }
                }
                
            }
        }

        private void OnClearRoom()
        {
            m_roomCount++;

                
        }

        public override void Deactivate()
        {
            EventService.RemoveListener(EventsDefinition.CLEAR_ROOM_ID, OnClearRoom);
        }
    }
}
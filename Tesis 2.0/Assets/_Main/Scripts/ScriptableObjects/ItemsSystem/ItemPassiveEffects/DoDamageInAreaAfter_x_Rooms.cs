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
        }

        private void OnClearRoom()
        {
            m_roomCount++;

            if (m_roomCount >= roomsCooldown)
            {
                EventService.DispatchEvent(EventsDefinition.CLEAR_ROOM_ID);
                m_roomCount = 0;
            }
        }

        public override void Deactivate()
        {
            EventService.RemoveListener(EventsDefinition.CLEAR_ROOM_ID, OnClearRoom);
        }
    }
}
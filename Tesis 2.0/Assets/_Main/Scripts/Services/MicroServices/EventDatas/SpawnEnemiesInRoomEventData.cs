using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services.MicroServices.EventsServices;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct SpawnEnemiesInRoomEventData : ICustomEventData
    {
        public Room Room { get; }

        public SpawnEnemiesInRoomEventData(Room p_room)
        {
            Room = p_room;
        }
    }
}
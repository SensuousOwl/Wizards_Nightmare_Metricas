using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.RoomsSystem;
using _Main.Scripts.Services.MicroServices.EventsServices;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct SpawnBossInRoomEventData : ICustomEventData
    {
        public Room Room { get; private set; }
        public List<EnemyModel> Bosses { get; private set; }
        public SpawnBossInRoomEventData(Room p_room, List<EnemyModel> p_bosses)
        {
            Room = p_room;
            Bosses = p_bosses;
        }
    }
}
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.RoomsSystem
{
    public class BossRoom : Room
    {
        [SerializeField] private GameObject activatePassLevel;

        private void Start()
        {
            activatePassLevel.SetActive(false);
        }

        protected override void EnterPlayerInRoom()
        {
            CloseDoors();
            EventService.DispatchEvent(new SpawnBossInRoom(this));
        }

        public override void ClearRoom()
        {
            base.ClearRoom();
            activatePassLevel.SetActive(true);
        }
    }

    public struct SpawnBossInRoom : ICustomEventData
    {
        public Room Room { get; private set; }
        
        public SpawnBossInRoom(Room p_room)
        {
            Room = p_room;
        }
    }
}
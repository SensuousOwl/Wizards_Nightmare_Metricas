using System.Collections.Generic;
using _Main.Scripts.Enemies;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.RoomsSystem
{
    public class BossRoom : Room
    {
        [SerializeField] private GameObject activatePassLevel;
        [SerializeField] private List<EnemyModel> bossesToSpawn;
        private void Start()
        {
            activatePassLevel.SetActive(false);
        }

        protected override void EnterPlayerInRoom()
        {
            CloseDoors();
            EventService.DispatchEvent(new SpawnBossInRoom(this,bossesToSpawn ));
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
        public List<EnemyModel> Bosses { get; private set; }
        public SpawnBossInRoom(Room p_room, List<EnemyModel> p_bosses)
        {
            Room = p_room;
            Bosses = p_bosses;
        }
    }
}
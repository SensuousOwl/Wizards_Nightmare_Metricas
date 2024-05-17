using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Services.MicroServices.EventDatas;
using UnityEngine;

namespace _Main.Scripts.RoomsSystem
{
    public class BossRoom : Room
    {
        [SerializeField] private GameObject activatePassLevel;
        [SerializeField] private BossHealthBarController bossHealthBar;
        [SerializeField] private List<EnemyModel> bossesToSpawn;

        
        private void Start()
        {
            activatePassLevel.SetActive(false);
        }

        protected override void SpawnEnemiesInRoom()
        {
            CloseDoors();
            EventService.DispatchEvent(new SpawnBossInRoomEventData(this,bossesToSpawn ));
        }

        public override void ClearRoom()
        {
            base.ClearRoom();
            activatePassLevel.SetActive(true);
        }

        public BossHealthBarController GetHealthBar() => bossHealthBar;
    }
}
using System;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Managers
{
    public class CheatsManager : MonoBehaviour
    {


        public static CheatsManager Singleton;

        private BossRoom m_bossRoom;
        private PlayerModel m_playerModel;
        private bool m_areCheatActive;

        private void Awake()
        {
#if !UNITY_EDITOR

            Destroy(gameObject);
            
#endif
            Singleton = this;
            DontDestroyOnLoad(this);
            
        }

        public void SubscribePlayer(PlayerModel p_model) => m_playerModel = p_model;
        public void SubscribeBossRoom(BossRoom p_model) => m_bossRoom = p_model;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                m_areCheatActive = true;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                m_playerModel.StatsController.SetUpgradeStat(StatsId.Damage, 20f);
                m_playerModel.StatsController.SetUpgradeStat(StatsId.FireRate, 0.20f);
                m_playerModel.StatsController.SetUpgradeStat(StatsId.MovementSpeed, 10);

            }
        }

        private void OnDrawGizmos()
        {
            if (!m_areCheatActive)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_playerModel.transform.position, m_bossRoom.transform.position);
        }
        
    }
}

using System;
using System.Collections.Generic;
using _Main.Scripts.Bullets;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ShootBurstToPlayer", menuName = "_main/States/Executions/ShootBurstToPlayer", order = 0)]
    public class ShootBurstToPlayerState : MyState
    {
        private class ThisData
        {
            public int BullCount;
            public float Timer;
        }
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int burstCount;
        [SerializeField] private float timeBtwBullets;

        private readonly Dictionary<EnemyModel, ThisData> m_models = new();

        private PoolGeneric<Bullet> m_bulletPool;

        public override void EnterState(EnemyModel p_model)
        {
            m_models[p_model] = new ThisData
            {
                BullCount = burstCount,
                Timer = 0f
            };

            m_bulletPool ??= new PoolGeneric<Bullet>(bulletPrefab);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            if (m_models[p_model].BullCount <= 0) 
                return;
            
            if (m_models[p_model].Timer <= Time.time)
            {
                var l_data = p_model.GetData();
                var l_dir = (LevelManager.Instance.PlayerModel.transform.position - p_model.transform.position)
                    .normalized;
                var l_bullet = m_bulletPool.GetorCreate();

                l_bullet.Initialize(p_model.transform.position, l_data.ProjectileSpeed, l_data.Damage, l_dir, l_data.Range, l_data.TargetMask);
                l_bullet.OnDeactivate -= OnDeactivateBulletHandler;
                p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");

                m_models[p_model].BullCount--;
                m_models[p_model].Timer = Time.time + timeBtwBullets;
            }
            else
            {
                p_model.SetIsAttacking(false);
            }
        }

        private void OnDeactivateBulletHandler(Bullet p_obj)
        {
            p_obj.OnDeactivate -= OnDeactivateBulletHandler;
            p_obj.gameObject.SetActive(false);
            m_bulletPool.AddPool(p_obj);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_models.Remove(p_model);
        }
    }
}

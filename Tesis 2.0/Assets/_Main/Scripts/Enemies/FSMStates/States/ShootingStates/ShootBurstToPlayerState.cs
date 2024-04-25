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

        private Dictionary<EnemyModel, ThisData> m_models = new Dictionary<EnemyModel, ThisData>();

        private PoolGeneric<Bullet> m_bulletPool;

        public override void EnterState(EnemyModel p_model)
        {
            m_models[p_model] = new ThisData();
            m_models[p_model].BullCount = burstCount;
            m_models[p_model].Timer = 0f;

            m_bulletPool ??= new PoolGeneric<Bullet>(bulletPrefab);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            if (m_models[p_model].BullCount > 0)
            {
                if (models[p_model].Timer <= Time.time)
                {
                    var data = p_model.GetData();
                    var dir = (LevelManager.Instance.PlayerModel.transform.position - p_model.transform.position).normalized;
                    var bul = Instantiate(bulletPrefab, p_model.transform.position, Quaternion.identity);
                    
                    bul.Initialize(data.ProjectileSpeed, data.Damage, dir, data.Range, data.TargetMask);
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

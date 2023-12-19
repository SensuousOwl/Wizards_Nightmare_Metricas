using System.Collections.Generic;
using _Main.Scripts.Bullets;
using _Main.Scripts.FSM.Base;
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

        private Dictionary<EnemyModel, ThisData> models = new Dictionary<EnemyModel, ThisData>();
        public override void EnterState(EnemyModel p_model)
        {
            models[p_model] = new ThisData();
            models[p_model].BullCount = burstCount;
            models[p_model].Timer = 0f;
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            if (models[p_model].BullCount > 0)
            {
                if (models[p_model].Timer <= Time.time)
                {
                    var data = p_model.GetData();
                    var dir = (p_model.GetTargetTransform().position - p_model.transform.position).normalized;
                    var bul = Instantiate(bulletPrefab, p_model.transform.position, Quaternion.identity);
                    
                    bul.Initialize(data.ProjectileSpeed, data.Damage, dir, data.Range, data.TargetMask);
                    p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");

                    models[p_model].BullCount--;
                    models[p_model].Timer = Time.time + timeBtwBullets;
                }
            }
            else
            {
                p_model.SetIsAttacking(false);
            }
        }

        public override void ExitState(EnemyModel p_model)
        {
            models.Remove(p_model);
        }
    }
}
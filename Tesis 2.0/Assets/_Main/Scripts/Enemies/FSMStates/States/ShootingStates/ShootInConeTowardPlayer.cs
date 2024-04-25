using _Main.Scripts.Bullets;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ShootInConeTowardPlayer", menuName = "_main/States/Executions/ShootInConeTowardPlayer", order = 0)]
    public class ShootInConeTowardPlayer : MyState
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int bulletsAmount;
        [SerializeField] private float totalConeAngle;
        
        private PoolGeneric<Bullet> m_bulletPool;

        public override void EnterState(EnemyModel p_model)
        {
            m_bulletPool ??= new PoolGeneric<Bullet>(bulletPrefab);
            
            p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");
            var l_diffAngle = totalConeAngle / bulletsAmount;
            var l_data = p_model.GetData();
            var l_dir = (LevelManager.Instance.PlayerModel.transform.position - p_model.transform.position).normalized;
            for (var l_i = 0; l_i < bulletsAmount; l_i++)
            {
                // "i % 2 == 0 ? -1 : 1" es para rotar el angulo del otro lado de la dir
                l_dir.RotateVector2(l_diffAngle * l_i * (l_i % 2 == 0 ? -1 : 1));

                //dir *= i % 2 == 0 ? -1 : 1;
                var l_bull = m_bulletPool.GetorCreate();
                l_bull.Initialize(p_model.transform.position, l_data.ProjectileSpeed, l_data.Damage, l_dir, l_data.Range, l_data.TargetMask);
                l_bull.OnDeactivate += OnDeactivateBulletHandler;
            }
            
            p_model.SetIsAttacking(false);
        }

        public override void ExecuteState(EnemyModel p_model) { }
        
        private void OnDeactivateBulletHandler(Bullet p_obj)
        {
            p_obj.OnDeactivate -= OnDeactivateBulletHandler;
            p_obj.gameObject.SetActive(false);
            m_bulletPool.AddPool(p_obj);
        }
    }
}

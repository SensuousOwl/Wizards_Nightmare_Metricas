using _Main.Scripts.Bullets;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.ShootingStates
{
    [CreateAssetMenu(fileName = "ShootInCirclesPattern", menuName = "_main/States/Executions/ShootInCirclesPattern", order = 0)]
    public class ShootInCirclesPattern : MyState
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int bulletsAmount;

        private PoolGeneric<Bullet> m_bulletPool;

        public override void EnterState(EnemyModel p_model)
        {
            m_bulletPool ??= new PoolGeneric<Bullet>(bulletPrefab);
            p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");
            var l_diffAngle = 360f / bulletsAmount;
            var l_data = p_model.GetData();
            for (var l_i = 0; l_i < bulletsAmount; l_i++)
            {
                var l_dir = Vector2.right.RotateVector2(l_diffAngle * l_i);
                Debug.Log(l_dir);
                var l_bull = m_bulletPool.GetorCreate();
                l_bull.Initialize(p_model.transform.position, l_data.ProjectileSpeed, l_data.Damage, l_dir, l_data.Range, l_data.TargetMask);
                l_bull.OnDeactivate += OnDeactivateBulletHandler;
            }
            
            p_model.SetIsAttacking(false);
        }

        private void OnDeactivateBulletHandler(Bullet p_obj)
        {
            p_obj.OnDeactivate -= OnDeactivateBulletHandler;
            p_obj.gameObject.SetActive(false);
            m_bulletPool.AddPool(p_obj);
        }

        public override void ExecuteState(EnemyModel p_model){}
    }
}
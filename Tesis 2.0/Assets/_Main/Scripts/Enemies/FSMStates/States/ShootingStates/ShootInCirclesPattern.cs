using _Main.Scripts.Bullets;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
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
            var l_diffAngle = 360 / bulletsAmount;
            var l_data = p_model.GetData();
            for (var l_i = 0; l_i < bulletsAmount; l_i++)
            {
                var l_dir = Vector2.right;
                l_dir.RotateVector2(l_diffAngle * l_i);
                
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
        
        private static Vector3 Rotate(Vector2 p_v, float p_delta) 
        {
            return new Vector2(
                p_v.x * Mathf.Cos(p_delta) - p_v.y * Mathf.Sin(p_delta),
                p_v.x * Mathf.Sin(p_delta) + p_v.y * Mathf.Cos(p_delta)
            );
        }
    }
}
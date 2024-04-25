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

        public override void EnterState(EnemyModel p_model)
        {
            p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");
            var diffAngle = 360 / bulletsAmount;
            var data = p_model.GetData();
            for (int i = 0; i < bulletsAmount; i++)
            {
                var dir = Vector2.right;
                dir = Rotate(dir, diffAngle * i * Mathf.Deg2Rad);
                var bull = Instantiate(bulletPrefab, p_model.transform.position, Quaternion.identity);
                bull.Initialize(data.ProjectileSpeed, data.Damage, dir, data.Range, data.TargetMask);
            }
            
            p_model.SetIsAttacking(false);
            
        }

        public override void ExecuteState(EnemyModel p_model){}
        
        public Vector3 Rotate(Vector2 v, float delta) 
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
    }
}
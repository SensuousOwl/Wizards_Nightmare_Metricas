using _Main.Scripts.Bullets;
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ShootInConeTowardPlayer", menuName = "_main/States/Executions/ShootInConeTowardPlayer", order = 0)]
    public class ShootInConeTowardPlayer : MyState
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int bulletsAmount;
        [SerializeField] private float totalConeAngle;

        public override void EnterState(EnemyModel p_model)
        {
            p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");
            var diffAngle = totalConeAngle / bulletsAmount;
            var data = p_model.GetData();
            var dir = (p_model.GetTargetTransform().position - p_model.transform.position).normalized;
            for (int i = 0; i < bulletsAmount; i++)
            {
                // "i % 2 == 0 ? -1 : 1" es para rotar el angulo del otro lado de la dir
                dir = Rotate(dir, diffAngle * i * (i % 2 == 0 ? -1 : 1) * Mathf.Deg2Rad);

                //dir *= i % 2 == 0 ? -1 : 1;
                
                var bull = Instantiate(bulletPrefab, p_model.transform.position, Quaternion.identity);
                bull.Initialize(data.ProjectileSpeed, data.Damage, dir, data.Range, data.TargetMask);
            }
            
            p_model.SetIsAttacking(false);
        }

        public override void ExecuteState(EnemyModel p_model) { }
        
        
        public Vector3 Rotate(Vector2 v, float delta) 
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
    }
}
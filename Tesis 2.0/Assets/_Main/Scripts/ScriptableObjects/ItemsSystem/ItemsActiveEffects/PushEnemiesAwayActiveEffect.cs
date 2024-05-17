using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemsActiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Active/PushEnemiesAwayActiveEffect")]
    public class PushEnemiesAwayActiveEffect : ItemActiveEffect
    {
        [SerializeField] private float radiusEffect;
        [SerializeField] private float pushForce;
        [SerializeField] private LayerMask targetMask;
        public override void UseItem()
        {
            var l_playerPos = PlayerModel.Local.transform.position;
            var l_col = new Collider2D[20];
            var l_count = Physics2D.OverlapCircleNonAlloc(l_playerPos, radiusEffect,  l_col, targetMask);

            for (int l_i = 0; l_i < l_count; l_i++)
            {
                if(!l_col[l_i].TryGetComponent(out EnemyModel l_enemyModel))
                    continue;

                var l_dir = (l_enemyModel.transform.position - l_playerPos).normalized;
                
                l_enemyModel.ApplyForce(l_dir, pushForce);
            }
        }
    }
}
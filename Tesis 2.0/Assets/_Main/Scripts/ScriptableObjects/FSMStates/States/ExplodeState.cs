using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "ExplodeState", menuName = "_main/States/Executions/ExplodeState", order = 0)]
    public class ExplodeState : MyState
    {
        [SerializeField] private float explosionRadius;
        [SerializeField] private LayerMask affetedExplotionMask;
        [SerializeField] private float explosionDamage = 40f;
        public override void EnterState(EnemyModel p_model)
        {
            p_model.SfxAudioPlayer.TryPlayRequestedClip("AttackID");
            var l_col = new Collider2D[20];
            var l_count = Physics2D.OverlapCircleNonAlloc(p_model.transform.position, explosionRadius,l_col , affetedExplotionMask);

            for (int l_i = 0; l_i < l_count; l_i++)
            {
                if (l_col[l_i].TryGetComponent(out PlayerModel l_playerModel))
                {
                    l_playerModel.HealthController.TakeDamage(p_model.GetData().Damage);
                    continue;
                }

                if (l_col[l_i].gameObject.layer == affetedExplotionMask)
                {
                    Destroy(l_col[l_i].gameObject);
                }
            }
            
            p_model.HealthController.TakeDamage(explosionDamage);
        }

        public override void ExecuteState(EnemyModel p_model) { }
    }
}
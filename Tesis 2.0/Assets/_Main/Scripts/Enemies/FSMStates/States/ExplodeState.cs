using _Main.Scripts.FSM.Base;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
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
            var col = new Collider2D[20];
            var count = Physics2D.OverlapCircleNonAlloc(p_model.transform.position, explosionRadius,col , affetedExplotionMask);

            for (int i = 0; i < count; i++)
            {
                if (col[i].TryGetComponent(out PlayerModel l_playerModel))
                {
                    l_playerModel.HealthController.TakeDamage(p_model.GetData().Damage);
                    continue;
                }

                if (col[i].gameObject.layer == affetedExplotionMask)
                {
                    Destroy(col[i].gameObject);
                }
            }
            
            //TODO; preguntar que onda con esto
            p_model.HealthController.TakeDamage(explosionDamage);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            throw new System.NotImplementedException();
        }
    }
}
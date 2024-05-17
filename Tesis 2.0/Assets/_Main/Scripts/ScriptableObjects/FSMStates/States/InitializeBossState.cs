using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States
{
    [CreateAssetMenu(fileName = "InitializeBossState", menuName = "_main/States/Executions/InitializeBossState", order = 0)]
    public class InitializeBossState : MyState
    {
        [SerializeField] private LayerMask RoomLayer;
        public override void EnterState(EnemyModel p_model)
        {
            var l_col = Physics.OverlapBox(p_model.transform.position, new Vector3(5,5,5), Quaternion.identity, RoomLayer);
            BossRoom l_room = null;
            for (int l_i = 0; l_i < l_col.Length; l_i++)
            {
                if (l_col[l_i].TryGetComponent(out BossRoom l_bossRoom))
                {
                    l_room = l_bossRoom;
                    break;
                }   
            }

            var l_hpBar = l_room.GetHealthBar();
            l_hpBar.Subscribe(p_model.HealthController);
        }

        public override void ExecuteState(EnemyModel p_model)
        {}
    }
}
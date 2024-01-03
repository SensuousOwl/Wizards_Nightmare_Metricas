using _Main.Scripts.FSM.Base;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "InitializeBossState", menuName = "_main/States/Executions/InitializeBossState", order = 0)]
    public class InitializeBossState : MyState
    {
        [SerializeField] private LayerMask RoomLayer;
        public override void EnterState(EnemyModel p_model)
        {
            var col = Physics.OverlapBox(p_model.transform.position, new Vector3(5,5,5), Quaternion.identity, RoomLayer);
            BossRoom room = null;
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].TryGetComponent(out BossRoom l_room))
                {
                    room = l_room;
                    break;
                }   
            }

            var hpBar = room.GetHealthBar();
            hpBar.Initialize(p_model.HealthController);
        }

        public override void ExecuteState(EnemyModel p_model)
        {}
    }
}
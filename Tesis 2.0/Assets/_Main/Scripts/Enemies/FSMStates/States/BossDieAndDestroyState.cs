using _Main.Scripts.FSM.Base;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "BossDieAndDestroyState", menuName = "_main/States/Executions/BossDieAndDestroyState", order = 0)]
    public class BossDieAndDestroyState : MyState
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
            hpBar.UnSubscribe(p_model.HealthController);
            
            p_model.TriggerDieEvent();
            Destroy(p_model.gameObject);
        }
        public override void ExecuteState(EnemyModel p_model)
        {
        }
    }
}
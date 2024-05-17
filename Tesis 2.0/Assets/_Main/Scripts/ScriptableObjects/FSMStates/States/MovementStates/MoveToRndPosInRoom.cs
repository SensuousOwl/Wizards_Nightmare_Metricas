using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "MoveToRndPosInRoom", menuName = "_main/States/Executions/Movement/MoveToRndPosInRoom", order = 0)]
    public class MoveToRndPosInRoom : MyState
    {
        [SerializeField] private LayerMask RoomLayer;
        private Dictionary<EnemyModel, Vector3> m_dictionary = new Dictionary<EnemyModel, Vector3>();
        public override void EnterState(EnemyModel p_model)
        {
            var l_col = Physics.OverlapBox(p_model.transform.position, new Vector3(5,5,5), Quaternion.identity, RoomLayer);
            BossRoom l_room = null;
            for (int i = 0; i < l_col.Length; i++)
            {
                if (l_col[i].TryGetComponent(out BossRoom l_bossRoom))
                {
                    l_room = l_bossRoom;
                    break;
                }   
            }
            
            var l_roomCenter = l_room.transform.position;
            var l_btmLeft= l_roomCenter - (Vector3)l_room.InsideRoomSize/2;
            var l_topRight= l_roomCenter + (Vector3)l_room.InsideRoomSize/2;
            
            var l_rndX = Random.Range(l_btmLeft.x, l_topRight.x);
            var l_rndY = Random.Range(l_btmLeft.y, l_topRight.y);
            m_dictionary[p_model] = new Vector3(l_rndX, l_rndY);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            var l_diff = m_dictionary[p_model] - p_model.transform.position;
            
            if(l_diff.magnitude <= 0.2f)
                return;
            
            p_model.MoveTowards(m_dictionary[p_model]);
        }

        public override void ExitState(EnemyModel p_model)
        {
            
            m_dictionary.Remove(p_model);
        }
    }
}
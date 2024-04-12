using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "MoveToRndPosInRoom", menuName = "_main/States/Executions/Movement/MoveToRndPosInRoom", order = 0)]
    public class MoveToRndPosInRoom : MyState
    {
        [SerializeField] private LayerMask RoomLayer;
        private Dictionary<EnemyModel, Vector3> m_dictionary = new Dictionary<EnemyModel, Vector3>();
        public override void EnterState(EnemyModel p_model)
        {
            var col = Physics.OverlapBox(p_model.transform.position, new Vector3(5,5,5), Quaternion.identity, RoomLayer);
            Room room = null;
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].TryGetComponent(out Room l_room))
                {
                    room = l_room;
                    break;
                }   
            }

            var roomCenter = room.transform.position;
            var btmLeft= roomCenter - (Vector3)room.InsideRoomSize/2;
            var topRight= roomCenter + (Vector3)room.InsideRoomSize/2;
            
            var rndX = Random.Range(btmLeft.x, topRight.x);
            var rndY = Random.Range(btmLeft.y, topRight.y);
            m_dictionary[p_model] = new Vector3(rndX, rndY);
            Debug.Log(m_dictionary[p_model]);
        }

        public override void ExecuteState(EnemyModel p_model)
        {

            var diff = m_dictionary[p_model] - p_model.transform.position;
            
            if(diff.magnitude <= 0.2f)
                return;
            
            p_model.MoveTowards(m_dictionary[p_model]);
        }

        public override void ExitState(EnemyModel p_model)
        {
            
            m_dictionary.Remove(p_model);
        }
    }
}
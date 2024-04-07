using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Managers;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "TeleportAroundPlayer", menuName = "_main/States/Executions/TeleportAroundPlayer", order = 0)]
    public class TeleportAroundTarget : MyState
    {
        private class data
        {
            public Room Room;
            public Vector2 TpPos;
            public float Timer;
        }
        
        [SerializeField] private Vector2 maxDistanceToTarget;
        [SerializeField] private Vector2 minDistanceToTarget;
        [SerializeField] private float timeToBanish;
        [SerializeField] private LayerMask RoomLayer;
        private Dictionary<EnemyModel, data> m_dictionary = new Dictionary<EnemyModel, data>();
        public override void EnterState(EnemyModel p_model)
        {
            var targetPos = LevelManager.Instance.PlayerModel.transform.position;
            m_dictionary[p_model] = new data();

            var col = Physics.OverlapBox(p_model.transform.position, Vector3.one, Quaternion.identity, RoomLayer);

            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].TryGetComponent(out Room l_room))
                {
                    m_dictionary[p_model].Room = l_room;
                    break;
                }   
            }
            m_dictionary[p_model].TpPos = CalcTransportPos(targetPos, minDistanceToTarget, maxDistanceToTarget, m_dictionary[p_model].Room);
            p_model.View.PlayTeleportAnim();
            //play start anim
        }


        public override void ExecuteState(EnemyModel p_model)
        {
            m_dictionary[p_model].Timer += Time.deltaTime;
            
            
            
            if(m_dictionary[p_model].Timer < timeToBanish)
                return;
            Debug.Log($"Tp a {m_dictionary[p_model].TpPos}");
            p_model.transform.position = m_dictionary[p_model].TpPos;
            p_model.SetIsAttacking(false);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_dictionary.Remove(p_model);
        }


        private Vector2 CalcTransportPos(Vector2 targetPos, Vector2 minDist, Vector2 maxDist, Room room)
        {
            var rndX = Random.Range(minDist.x, maxDist.x);
            var rndY = Random.Range(minDist.y, maxDist.y);

            var TpPos = targetPos + new Vector2(rndX, rndY);
            if (room.IsInsideBounds(TpPos))
                return TpPos;
            
            return CalcTransportPos(targetPos, minDist, maxDistanceToTarget, room);
        }
    }
}
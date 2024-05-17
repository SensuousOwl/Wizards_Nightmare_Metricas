using System.Collections.Generic;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.FSM;
using _Main.Scripts.RoomsSystem;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "TeleportAroundPlayer", menuName = "_main/States/Executions/Movement/TeleportAroundPlayer", order = 0)]
    public class TeleportAroundTarget : MyState
    {
        private class Data
        {
            public Room Room;
            public Vector2 TpPos;
            public float Timer;
        }
        
        [SerializeField] private Vector2 maxDistanceToTarget;
        [SerializeField] private Vector2 minDistanceToTarget;
        [SerializeField] private float timeToBanish;
        [SerializeField] private LayerMask roomLayer;
        private Dictionary<EnemyModel, Data> m_dictionary = new Dictionary<EnemyModel, Data>();
        public override void EnterState(EnemyModel p_model)
        {
            var l_player = PlayerModel.Local;
            var l_targetPos = Vector2.zero;

            if (l_player != default)
            {
                l_targetPos = l_player.transform.position;
            }
            
            m_dictionary[p_model] = new Data();

            var l_col = Physics.OverlapBox(p_model.transform.position, Vector3.one, Quaternion.identity, roomLayer);

            for (int l_i = 0; l_i < l_col.Length; l_i++)
            {
                if (l_col[l_i].TryGetComponent(out Room l_room))
                {
                    m_dictionary[p_model].Room = l_room;
                    break;
                }   
            }
            m_dictionary[p_model].TpPos = CalcTransportPos(l_targetPos, minDistanceToTarget, maxDistanceToTarget, m_dictionary[p_model].Room);
            p_model.View.PlayTeleportAnim();
            //play start anim
        }


        public override void ExecuteState(EnemyModel p_model)
        {
            m_dictionary[p_model].Timer += Time.deltaTime;
            
            
            
            if(m_dictionary[p_model].Timer < timeToBanish)
                return;
            
            p_model.transform.position = m_dictionary[p_model].TpPos;
            p_model.SetIsAttacking(false);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_dictionary.Remove(p_model);
        }


        private Vector2 CalcTransportPos(Vector2 p_targetPos, Vector2 p_minDist, Vector2 p_maxDist, Room p_room)
        {
            var l_rndX = Random.Range(p_minDist.x, p_maxDist.x);
            var l_rndY = Random.Range(p_minDist.y, p_maxDist.y);

            var l_tpPos = p_targetPos + new Vector2(l_rndX, l_rndY);
            if (p_room.IsInsideBounds(l_tpPos))
                return l_tpPos;
            
            return CalcTransportPos(p_targetPos, p_minDist, maxDistanceToTarget, p_room);
        }
    }
}
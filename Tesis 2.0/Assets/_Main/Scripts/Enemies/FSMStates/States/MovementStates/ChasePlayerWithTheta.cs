using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Extensions;
using _Main.Scripts.FSM.Base;
using _Main.Scripts.Grid;
using _Main.Scripts.Managers;
using _Main.Scripts.PathFinding;
using _Main.Scripts.Steering_Behaviours;
using Unity.VisualScripting;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.States.MovementStates
{
    [CreateAssetMenu(fileName = "ChasePlayerWithTheta", menuName = "_main/States/Executions/Movement/ChasePlayerWithTheta", order = 0)]
    public class ChasePlayerWithTheta : MyState
    {
        [SerializeField] private LayerMask obsMask;
        [SerializeField] private float avoidForce = 10f;
        [SerializeField] private float refreshPathTimer;
        [SerializeField] private float enemyRadius = 0.5f;
        private class Data
        {
            public MyNodeGrid Grid;
            public List<MyNode> Path;
            public float RefreshTime; 
            public int NodeCount;
            public Transform TargetTransform;

        }
        
        private Dictionary<EnemyModel, Data> m_dictionary = new Dictionary<EnemyModel, Data>();
        public override void EnterState(EnemyModel p_model)
        {
            m_dictionary[p_model] = new Data();
            m_dictionary[p_model].Grid = p_model.MyRoom.Grid;
            m_dictionary[p_model].TargetTransform = LevelManager.Instance.PlayerModel.transform;
            RecalculatePath(p_model);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            //Si hay linea recta hasta el player sin obstaculos, anda en linea recta
            var l_diff = (m_dictionary[p_model].TargetTransform.position- p_model.transform.position);
            
            if(!Physics2D.CircleCast(p_model.transform.position, enemyRadius, l_diff.normalized, 
                   l_diff.magnitude, obsMask, -0.5f, 0.5f))
            {
                Debug.Log("LINEA RECTA");
                p_model.MoveTowards(m_dictionary[p_model].TargetTransform.position);
                return;
            }
            //Si hay obstaculos en el camino, hagamos el pathfinding 
            
            MoveWithTheta(p_model);
        }

        private void MoveWithTheta(EnemyModel p_model)
        {
            if((m_dictionary[p_model].Path.Count == 0))
            {
                //Si entra aca, es que el path fue "nulo" / vacio
                RecalculatePath(p_model);
                return;
            }
            //Pasado x tiempo, hay que volver a carcular el camino
            if (m_dictionary[p_model].RefreshTime < Time.time)
            {
                RecalculatePath(p_model);
                return;
            }

            var l_nodeCount = m_dictionary[p_model].NodeCount;
            var l_targetNode = m_dictionary[p_model].Path[l_nodeCount];
            var l_distanceToNodeTarget =
                Vector3.Distance(p_model.transform.position, l_targetNode.WorldPos);
            
            
            if (l_distanceToNodeTarget <= 0.2f && l_nodeCount < m_dictionary[p_model].Path.Count)
            {
                m_dictionary[p_model].NodeCount++;
            }
            else if (m_dictionary[p_model].NodeCount >= m_dictionary[p_model].Path.Count)
            {
                //Significa que llego al ultimo nodo
                RecalculatePath(p_model);
                return;
            }

            var l_currentTargetNode = m_dictionary[p_model].Path[m_dictionary[p_model].NodeCount];

            var l_wantedDir = MySteeringBehaviors.GetAdvancedObsAvoidanceDir(p_model.transform.position,
                l_currentTargetNode.WorldPos, p_model.GetData().ObsDetectionRadius, avoidForce, obsMask);
            
            //p_model.Move(wantedDir);
            p_model.MoveTowards(l_currentTargetNode.WorldPos);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_dictionary[p_model] = default;
            m_dictionary.Remove(p_model);
        }

        private void RecalculatePath(EnemyModel p_model)
        {
            var l_grid = m_dictionary[p_model].Grid;
            var l_myNodePos = l_grid.NodeFromWorldPoint(p_model.transform.position);
            var l_allNeigh = l_grid.GetNeighbours(l_myNodePos);
            var l_targetPos = l_grid.NodeFromWorldPoint(m_dictionary[p_model].TargetTransform.position);
            
            
            
            m_dictionary[p_model].Path = ThetaStar.RunCustomGrid(l_allNeigh.ToList(), l_targetPos,m_dictionary[p_model].Grid, obsMask,
                PSatisfies, PConnections, PGetCost, PHeuristic, PInView);

            m_dictionary[p_model].NodeCount = 0;
            m_dictionary[p_model].RefreshTime = Time.time + refreshPathTimer;
        }

        #region ThetaFunks

        private bool PInView(MyNode p_from, MyNode p_to, LayerMask p_targetMask)
        {
            var l_diff = (p_from.WorldPos - p_to.WorldPos);


            return Physics2D.CircleCast(p_from.WorldPos, 0.25f, l_diff.normalized, l_diff.magnitude, p_targetMask);
        }

        private float PHeuristic(MyNode p_startNode, MyNode p_endNode)
        {
            return Vector3.Distance(p_startNode.WorldPos, p_endNode.WorldPos);
        }

        private float PGetCost(MyNode p_startNode, MyNode p_endNode)
        {
            return Vector3.Distance(p_startNode.WorldPos, p_endNode.WorldPos);
        }

        private IEnumerable<MyNode> PConnections(MyNodeGrid p_grid,MyNode p_arg)
        {
            return p_grid.GetNeighbours(p_arg);
        } 

        private bool PSatisfies(MyNode p_curr, MyNode p_target)
        {
            return p_curr == p_target;
        }

        #endregion
        
        
    }
}
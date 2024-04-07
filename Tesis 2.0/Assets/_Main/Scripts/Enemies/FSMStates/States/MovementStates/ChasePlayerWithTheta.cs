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
    [CreateAssetMenu(fileName = "ChasePlayerWithTheta", menuName = "_main/States/Executions/ChasePlayerWithTheta", order = 0)]
    public class ChasePlayerWithTheta : MyState
    {
        [SerializeField] private LayerMask obsMask;
        [SerializeField] private float avoidForce = 10f;
        [SerializeField] private float refreshPathTimer;
        private class Data
        {
            public MyNodeGrid grid;
            public List<MyNode> path;
            public float refreshTime; 
            public int nodeCount;
            public Transform targetTransform;

        }
        
        private Dictionary<EnemyModel, Data> m_dictionary = new Dictionary<EnemyModel, Data>();
        public override void EnterState(EnemyModel p_model)
        {
            m_dictionary[p_model] = new Data();
            m_dictionary[p_model].grid = p_model.NodeGrid;
            m_dictionary[p_model].targetTransform = LevelManager.Instance.PlayerModel.transform;
            RecalculatePath(p_model);
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            //Si hay linea recta hasta el player sin obstaculos, anda en linea recta
            var diff = (p_model.transform.position - m_dictionary[p_model].targetTransform.position);
            
            Debug.Log(!Physics2D.CircleCast(p_model.transform.position, 0.5f, diff.normalized, 
                diff.magnitude, obsMask, -0.5f, 0.5f));
            
            if(!Physics2D.CircleCast(p_model.transform.position, 0.5f, diff.normalized, 
                   diff.magnitude, obsMask, -0.5f, 0.5f))
            {
                p_model.MoveTowards(m_dictionary[p_model].targetTransform.position);
                return;
            }
            //Si hay obstaculos en el camino, hagamos el pathfinding 
            
            MoveWithTheta(p_model);
        }

        private void MoveWithTheta(EnemyModel p_model)
        {
            if((m_dictionary[p_model].path.Count == 0))
            {
                //Si entra aca, es que el path fue "nulo" / vacio
                RecalculatePath(p_model);
                return;
            }
            //Pasado x tiempo, hay que volver a carcular el camino
            if (m_dictionary[p_model].refreshTime < Time.time)
            {
                RecalculatePath(p_model);
                return;
            }

            var nodeCount = m_dictionary[p_model].nodeCount;
            var targetNode = m_dictionary[p_model].path[nodeCount];
            var distanceToNodeTarget =
                Vector3.Distance(p_model.transform.position, targetNode.WorldPos);
            
            
            if (distanceToNodeTarget <= 0.2f && nodeCount < m_dictionary[p_model].path.Count)
            {
                m_dictionary[p_model].nodeCount++;
            }
            else if (m_dictionary[p_model].nodeCount >= m_dictionary[p_model].path.Count)
            {
                //Significa que llego al ultimo nodo
                RecalculatePath(p_model);
                return;
            }

            var currentTargetNode = m_dictionary[p_model].path[m_dictionary[p_model].nodeCount];

            var wantedDir = MySteeringBehaviors.GetAdvancedObsAvoidanceDir(p_model.transform.position,
                currentTargetNode.WorldPos, p_model.GetData().ObsDetectionRadius, avoidForce, obsMask);
            
            //p_model.Move(wantedDir);
            p_model.MoveTowards(currentTargetNode.WorldPos);
        }

        public override void ExitState(EnemyModel p_model)
        {
            m_dictionary[p_model] = default;
            m_dictionary.Remove(p_model);
        }

        private void RecalculatePath(EnemyModel p_model)
        {
            var grid = m_dictionary[p_model].grid;
            var myNodePos = grid.NodeFromWorldPoint(p_model.transform.position);
            var targetPos = grid.NodeFromWorldPoint(m_dictionary[p_model].targetTransform.position);
            
            m_dictionary[p_model].path = ThetaStar.RunCustomGrid(myNodePos, targetPos,m_dictionary[p_model].grid, obsMask,
                PSatisfies, PConnections, PGetCost, PHeuristic, PInView);

            m_dictionary[p_model].nodeCount = 0;
            m_dictionary[p_model].refreshTime = Time.time + refreshPathTimer;
        }

        #region ThetaFunks

        private bool PInView(MyNode p_from, MyNode p_to, LayerMask p_targetMask)
        {
            var diff = (p_from.WorldPos - p_to.WorldPos);


            return Physics2D.CircleCast(p_from.WorldPos, 0.25f, diff.normalized, diff.magnitude, p_targetMask);
        }

        private float PHeuristic(MyNode p_startNode, MyNode p_endNode)
        {
            return Vector3.Distance(p_startNode.WorldPos, p_endNode.WorldPos);
        }

        private float PGetCost(MyNode p_startNode, MyNode p_endNode)
        {
            return Vector3.Distance(p_startNode.WorldPos, p_endNode.WorldPos);
        }

        private List<MyNode> PConnections(MyNodeGrid grid,MyNode arg)
        {
            return grid.GetNeighbours(arg).ToList();
        } 

        private bool PSatisfies(MyNode p_curr, MyNode p_target)
        {
            return p_curr == p_target;
        }

        #endregion
        
        
    }
}
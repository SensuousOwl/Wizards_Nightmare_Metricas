﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Main.Scripts.Grid
{
    public class MyNodeGrid : MonoBehaviour
    {
        [SerializeField] private MyNode myNodePrefab;
        [SerializeField] private LayerMask unWalkableMask;
        [SerializeField] private Vector2 gridworldSize;
        [SerializeField] private float nodeRadius = 1f;
        [SerializeField] private float ySpacing = 1f;
        
        
        private MyNode [,] m_grid;
        private float m_nodeDiameter;
        private int m_gridSizeX;
        private int m_gridSizeY;


        private void Awake()
        {
            Initialize();
        }


        [ContextMenu("GenerateGrid")]
        private void Initialize()
        {
            
            m_nodeDiameter = nodeRadius*2;
            m_gridSizeX = Mathf.RoundToInt(gridworldSize.x/m_nodeDiameter);
            m_gridSizeY = Mathf.RoundToInt(gridworldSize.y/m_nodeDiameter);
            CreateGrid();
        }
        
        void CreateGrid()
        {
            m_grid = new MyNode[m_gridSizeX, m_gridSizeY];
            Vector3 l_worldBottomLeft = transform.position - Vector3.right * gridworldSize.x / 2 -
                                      Vector3.up * gridworldSize.y / 2;

            var l_halfExtents = new Vector2(m_nodeDiameter,m_nodeDiameter);
            for (int l_x = 0; l_x < m_gridSizeX; l_x++)
            {
                for (int l_y = 0; l_y < m_gridSizeY; l_y++)
                {
                    //Fijamos la verdadera posicion en el mundo del nodo
                    Vector3 l_worldPoint = l_worldBottomLeft + Vector3.right * (l_x * m_nodeDiameter + nodeRadius) + Vector3.up * ySpacing * ((l_y) * m_nodeDiameter + nodeRadius);
                    bool l_walkable = !Physics2D.OverlapBox(l_worldPoint, l_halfExtents, 0, unWalkableMask);
                    var l_node = new MyNode();
                    l_node.Initialize(l_walkable, l_worldPoint, m_nodeDiameter/2 , new Vector3(l_x, l_y));

                        
                    m_grid[l_x, l_y] = l_node;
                }
            }
        }

       
        public MyNode NodeFromWorldPoint(Vector3 p_worldPosition)
        {
            var l_position = transform.position;
            var l_percentX = ((p_worldPosition.x - l_position.x) + gridworldSize.x / 2) / gridworldSize.x;
            var l_percentY = ((p_worldPosition.y - l_position.y) + gridworldSize.y / 2) / gridworldSize.y;
            
            
            l_percentX = Mathf.Clamp01(l_percentX);
            l_percentY = Mathf.Clamp01(l_percentY);

            var l_x = Mathf.RoundToInt((m_gridSizeX - 1) * l_percentX);
            var l_y = Mathf.RoundToInt((m_gridSizeY - 1) * l_percentY);
            return m_grid[l_x, l_y];
        }
        
        public IEnumerable<MyNode> GetNeighbours(MyNode p_node)
        {
            var l_neighbours = new List<MyNode>();

            if (p_node.XId-1 >-1)
                l_neighbours.Add(m_grid[(p_node.XId - 1), p_node.YId]);

            if (p_node.XId + 1 <= m_gridSizeX-1)
                l_neighbours.Add(m_grid[(p_node.XId + 1), p_node.YId]);
            
            if (p_node.YId - 1 > -1)
                l_neighbours.Add(m_grid[p_node.XId, (p_node.YId-1)]);

            if (p_node.YId + 1 <= m_gridSizeY-1)
                l_neighbours.Add(m_grid[p_node.XId, (p_node.YId + 1)]);
            

            return l_neighbours.Where(p_x=> p_x.Walkable);
        }
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridworldSize.x, gridworldSize.y));

            if (m_grid != null)
            {
                foreach (var l_node in m_grid)
                {
                    Gizmos.color = (l_node.Walkable) ? Color.green : Color.red;
                    Gizmos.DrawWireCube(l_node.WorldPos, Vector3.one * (m_nodeDiameter- 0.1f));
                }
            }
        }
#endif
        
    }
}
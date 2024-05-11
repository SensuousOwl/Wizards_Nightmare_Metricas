using System;
using System.Collections.Generic;
using _Main.Scripts.Extensions;
using _Main.Scripts.Grid;
using UnityEngine;

namespace _Main.Scripts.PathFinding
{
    public static class ThetaStar
    {
        public static IEnumerable<T> Run<T>(T p_start, T p_target, 
            Func<T, T, bool> p_satisfies,
            Func<T, List<T>> p_connections,
            Func<T, T, float> p_getCost,
            Func<T,T, float> p_heuristic,
            Func<T, T, bool> p_inView,
            int p_watchdog = 2000)
        {
            PriorityQueue<T> l_pending = new PriorityQueue<T>();
            HashSet<T> l_visited = new HashSet<T>();
            Dictionary<T, T> l_parent = new Dictionary<T, T>();
            Dictionary<T, float> l_cost = new Dictionary<T, float>();

            l_pending.Enqueue(p_start, 0);
            l_cost[p_start] = 0;

            while (p_watchdog > 0 && !l_pending.IsEmpty)
            {
                p_watchdog--;
                var l_curr = l_pending.Dequeue();
                
                if (p_satisfies(l_curr, p_target))
                {
                    var l_path = new List<T>();
                    l_path.Add(l_curr);
                    while (l_parent.ContainsKey(l_path[l_path.Count - 1]))
                    {
                        var l_father = l_parent[l_path[l_path.Count - 1]];
                        l_path.Add(l_father);
                    }
                    l_path.Reverse();
                    return l_path;
                }
                l_visited.Add(l_curr);
                var l_neighbours = p_connections(l_curr);
                for (int l_i = 0; l_i < l_neighbours.Count; l_i++)
                {
                    var l_neigh = l_neighbours[l_i];
                    if (l_visited.Contains(l_neigh)) continue;
                    T l_realParent = l_curr;
                    if (l_parent.ContainsKey(l_curr) && p_inView(l_parent[l_curr], l_neigh))
                    {
                        l_realParent = l_parent[l_curr];
                    }
                    float l_tentativeCost = l_cost[l_realParent] + p_getCost(l_realParent, l_neigh);
                    if (l_cost.ContainsKey(l_neigh) && l_cost[l_neigh] < l_tentativeCost) continue;
                    l_pending.Enqueue(l_neigh, l_tentativeCost + p_heuristic(l_neigh, p_target));
                    l_parent[l_neigh] = l_realParent;
                    l_cost[l_neigh] = l_tentativeCost;
                }
            }
            return new List<T>();
        }
        
        public static List<MyNode> RunCustomGrid(List<MyNode> p_startingNodes, MyNode p_target, MyNodeGrid p_grid, LayerMask p_obsMask, 
            Func<MyNode, MyNode, bool> p_Satisfies,
            Func<MyNodeGrid,MyNode, IEnumerable<MyNode>> p_Connections,
            Func<MyNode, MyNode, float> p_GetCost,
            Func<MyNode,MyNode, float> p_Heuristic,
            Func<MyNode, MyNode,LayerMask, bool> p_InView,
            int p_watchdog = 2000)
        {
            PriorityQueue<MyNode> l_pending = new PriorityQueue<MyNode>();
            HashSet<MyNode> l_visited = new HashSet<MyNode>();
            Dictionary<MyNode, MyNode> l_parent = new Dictionary<MyNode, MyNode>();
            Dictionary<MyNode, float> l_cost = new Dictionary<MyNode, float>();

            var l_startNode = p_startingNodes[0];
            var l_previousCost = p_GetCost(p_target, l_startNode);
            for (int i = 0; i < p_startingNodes.Count; i++)
            {
                var l_currNode = p_startingNodes[i];
                
                if(!l_currNode.Walkable)
                    continue;
                
                var l_currCost = p_GetCost(p_target, l_currNode);
                if (l_previousCost > l_currCost)
                {
                    l_startNode = l_currNode;
                    l_previousCost = l_currCost;
                }
            }
            
            l_pending.Enqueue(l_startNode, 0);
            l_cost[l_startNode] = 0;

            while (p_watchdog > 0 && !l_pending.IsEmpty)
            {
                p_watchdog--;
                var l_curr = l_pending.Dequeue();
                
                if (p_Satisfies(l_curr, p_target))
                {
                    var l_path = new List<MyNode>();
                    l_path.Add(l_curr);
                    while (l_parent.ContainsKey(l_path[l_path.Count - 1]))
                    {
                        var l_father = l_parent[l_path[l_path.Count - 1]];
                        l_path.Add(l_father);
                    }
                    l_path.Reverse();
                    return l_path;
                }
                l_visited.Add(l_curr);
                var l_neighbours = p_Connections(p_grid,l_curr);

                foreach (var l_neigh in l_neighbours)
                {
                    if (l_visited.Contains(l_neigh)) continue;
                    MyNode l_realParent = l_curr;
                    if (l_parent.ContainsKey(l_curr) && p_InView(l_parent[l_curr], l_neigh, p_obsMask))
                    {
                        l_realParent = l_parent[l_curr];
                    }
                    float l_tentativeCost = l_cost[l_realParent] + p_GetCost(l_realParent, l_neigh);
                    if (l_cost.ContainsKey(l_neigh) && l_cost[l_neigh] < l_tentativeCost) continue;
                    l_pending.Enqueue(l_neigh, l_tentativeCost + p_Heuristic(l_neigh, p_target));
                    l_parent[l_neigh] = l_realParent;
                    l_cost[l_neigh] = l_tentativeCost;
                }
            }
            return new List<MyNode>();
        }
    }
}


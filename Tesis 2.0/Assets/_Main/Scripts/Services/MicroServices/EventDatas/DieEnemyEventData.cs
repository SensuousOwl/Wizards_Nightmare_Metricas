using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Services.MicroServices.EventsServices;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct DieEnemyEventData : ICustomEventData
    {
        public EnemyModel Model { get; }
        public Vector3 PositionNode { get; }
        
        public DieEnemyEventData(Vector3 p_positionNode, EnemyModel p_model)
        {
            PositionNode = p_positionNode;
            Model = p_model;
        }
    }
}
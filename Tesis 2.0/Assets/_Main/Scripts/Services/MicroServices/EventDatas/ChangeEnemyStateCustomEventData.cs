using System;
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.Services.MicroServices.EventsServices;

namespace _Main.Scripts.Services.MicroServices.EventDatas
{
    public struct ChangeEnemyStateCustomEventData : ICustomEventData
    {
        public EnemyModel Model { get; }
        public Type StateType { get; }

        public ChangeEnemyStateCustomEventData(EnemyModel p_model, Type p_stateType)
        {
            Model = p_model;
            StateType = p_stateType;
        }
    }
}
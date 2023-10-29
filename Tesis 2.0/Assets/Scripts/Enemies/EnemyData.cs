using System.Collections.Generic;
using Bullets;
using FSM.Base;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "_main/Data/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        
        [field: SerializeField] public List<StateData> AllStatesData { get; private set; }
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public float ViewDepthRange { get; private set; }
        
        
        [field: SerializeField] public PlayerBullet Bullet { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public LayerMask TargetMask { get; private set; }
    }
}
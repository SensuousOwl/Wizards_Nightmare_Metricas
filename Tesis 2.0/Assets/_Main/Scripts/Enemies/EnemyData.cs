using System.Collections.Generic;
using _Main.Scripts.Bullets;
using _Main.Scripts.FSM.Base;
using UnityEngine;

namespace _Main.Scripts.Enemies
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
        
        
        [field: SerializeField] public Bullet Bullet { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Xp { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public LayerMask TargetMask { get; private set; }
        [field: SerializeField] public float ExperienceDrop { get; private set; }
        [field: Header("PathFinding")]
        [field: SerializeField] public float ObsDetectionRadius { get; private set; }
        [field: SerializeField] public float ObsDetectionAngle { get; private set; }
        
    }
}
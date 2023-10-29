using FSM.Base;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        public Transform GetTargetTransform()
        {
            return transform;
        }

        public EnemyData GetData() => data;

        public void SetLastTargetLocation(Vector3 p_pos)
        {
            
        }

    }
}
using Extensions;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private GameObject TEST_PLAYER;

        private int m_currHp;

        public IHealthController HealthController { get; private set; }
        
        private void Awake()
        {
            HealthController = GetComponent<HealthController>();
            HealthController.Initialize(data.MaxHp);
            HealthController.OnDie += Die;
        }

        public Transform GetTargetTransform()
        {
            return TEST_PLAYER.transform;
        }

        public EnemyData GetData() => data;

        public void SetLastTargetLocation(Vector3 p_pos)
        {
            
        }

        public void MoveTowards(Vector3 p_targetPoint)
        {
            //TODO: Agregar sistemas de obs avoidance
            p_targetPoint.Xyo();
            var dir = (p_targetPoint - transform.position).normalized;
            transform.position += dir * (data.MovementSpeed * Time.deltaTime);
            Debug.Log($"Targer : {p_targetPoint}, dir {dir}, movement {dir * (data.MovementSpeed * Time.deltaTime)}");
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision p_collision)
        {
            if (!p_collision.gameObject.layer.Equals(data.TargetMask)) 
                return;
            
            if(!p_collision.gameObject.TryGetComponent(out IHealthController l_healthController))
                return;
                
            l_healthController.TakeDamage(data.Damage);
        }
    }
}
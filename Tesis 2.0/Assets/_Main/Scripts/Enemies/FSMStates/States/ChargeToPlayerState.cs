using System.Collections.Generic;
using _Main.Scripts.FSM.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.Enemies.FSMStates.States
{
    [CreateAssetMenu(fileName = "ChargeToPlayerState", menuName = "_main/States/Executions/ChargeToPlayerState", order = 0)]
    public class ChargeToPlayerState : MyState
    {
        private class ThisData
        {
            public Vector3 Destination;
            public float PrepTime;
            public float Timer;
        }
        [SerializeField] private float dashSpeed;
        [SerializeField] private float prepTime;
        [SerializeField] private float maxTimeToReachTarget;
        
        private Dictionary<EnemyModel, ThisData> models = new Dictionary<EnemyModel, ThisData>();

        public override void EnterState(EnemyModel p_model)
        {
            var l_destination = p_model.GetTargetTransform().position;
            var l_data = new ThisData();
            l_data.Destination = l_destination;
            l_data.PrepTime = Time.time + prepTime;
            models[p_model] = l_data;
        }

        public override void ExecuteState(EnemyModel p_model)
        {
            
            if (models[p_model].PrepTime >= Time.time)
            {
                p_model.transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
                return;
            }

            models[p_model].Timer += Time.deltaTime;
            p_model.transform.rotation = Quaternion.identity;
            var l_diff = models[p_model].Destination - p_model.transform.position;
            p_model.transform.position += l_diff.normalized * (dashSpeed * Time.deltaTime);


            if (l_diff.magnitude <= 0.2f || models[p_model].Timer >= maxTimeToReachTarget)
            {
                p_model.SetIsAttacking(false);
            }
        }

        public override void ExitState(EnemyModel p_model)
        {
            models.Remove(p_model);
        }
    }
}
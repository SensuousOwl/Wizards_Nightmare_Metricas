using _Main.Scripts.FSM.Base;
using Unity.VisualScripting;
using UnityEngine;

namespace _Main.Scripts.Enemies.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "HpUnder_X_percentage", menuName = "_main/States/Conditions/HpUnder_X_percentage", order = 0)]
    public class HpUnderXpercentage : StateCondition
    {
        [SerializeField] private float percentage;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var hc = p_model.HealthController;
            return (hc.GetCurrentHealth() / hc.GetMaxHealth()) * 100 <= percentage;
        }
    }
}
using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "HpUnder_X_percentage", menuName = "_main/States/Conditions/HpUnder_X_percentage", order = 0)]
    public class HpUnderXpercentage : StateCondition
    {
        [SerializeField] private float percentage;
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var l_hc = p_model.HealthController;
            return (l_hc.GetCurrentHealth() / l_hc.GetMaxHealth()) * 100 <= percentage;
        }
    }
}
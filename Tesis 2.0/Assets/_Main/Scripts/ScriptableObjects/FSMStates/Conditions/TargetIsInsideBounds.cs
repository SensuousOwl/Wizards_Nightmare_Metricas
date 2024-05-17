using _Main.Scripts.Entities.Enemies.MVC;
using _Main.Scripts.FSM;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.FSMStates.Conditions
{
    [CreateAssetMenu(fileName = "TargetIsInsideBounds", menuName = "_main/States/Conditions/TargetIsInsideBounds", order = 0)]
    public class TargetIsInsideBounds : StateCondition
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] private Vector3 offsetFromModel;
        [SerializeField] private Vector3 boundSize;

        private Collider2D[] m_collider2Ds = new Collider2D[20];
        public override bool CompleteCondition(EnemyModel p_model)
        {
            var l_col = Physics2D.OverlapBoxNonAlloc(p_model.transform.position + offsetFromModel, boundSize, 0,
                m_collider2Ds, layer);

            return l_col > 0;
        }
    }
}
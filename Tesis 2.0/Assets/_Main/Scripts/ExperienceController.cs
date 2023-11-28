using _Main.Scripts.Enemies;
using _Main.Scripts.UI;
using UnityEngine;

namespace _Main.Scripts
{
    public class ExperienceController : MonoBehaviour
    {
        [SerializeField] private UpgradeScreenController upgradeScreenController;

        private float m_experience;
        private float m_experienceToUpgradeLevel = 100f;
        private int m_level;
        private float m_newLevel = 3;

        private void OnEnable()
        {
            EnemyModel.OnDie += EnemyModelOnOnDie;
        }

        private void OnDisable()
        {
            EnemyModel.OnDie -= EnemyModelOnOnDie;
        }
        
        private void EnemyModelOnOnDie(EnemyModel p_obj)
        {
            m_experience += p_obj.GetData().ExperienceDrop;

            if (m_experience < m_experienceToUpgradeLevel)
                return;

            m_experience -= m_experienceToUpgradeLevel;
            m_experienceToUpgradeLevel += m_experienceToUpgradeLevel * 0.1f;
            m_level++;
            
            if (m_level < m_newLevel)
                return;

            m_newLevel += 3;
            upgradeScreenController.ActivateUpgradeScreen();
        }
    }
}
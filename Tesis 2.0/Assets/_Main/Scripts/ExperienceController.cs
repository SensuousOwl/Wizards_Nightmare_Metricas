using _Main.Scripts.Enemies;
using _Main.Scripts.UI.Menus;
using UnityEngine;

namespace _Main.Scripts
{
    public class ExperienceController : MonoBehaviour
    {
        [SerializeField] private int countToNewLevel = 3;
        [SerializeField] private float experienceToUpgradeLevel = 100f;
        [SerializeField] private UpgradeScreenController upgradeScreenController;

        private float m_experience;
        private int m_level;
        private int m_newLevel = 1;

        private void OnEnable()
        {
            EnemyModel.OnExperienceDrop += EnemyModelOnOnDie;
        }

        private void OnDisable()
        {
            EnemyModel.OnExperienceDrop -= EnemyModelOnOnDie;
        }
        
        private void EnemyModelOnOnDie(float p_experienceDrop)
        {
            m_experience += p_experienceDrop;
            if (m_experience < experienceToUpgradeLevel)
                return;
            
            m_experience -= experienceToUpgradeLevel;
            experienceToUpgradeLevel += experienceToUpgradeLevel * 0.1f;
            m_level++;
            
            if (m_level < m_newLevel)
                 return;
            
            m_newLevel += countToNewLevel;
            upgradeScreenController.ActivateUpgradeScreen();
        }
    }
}
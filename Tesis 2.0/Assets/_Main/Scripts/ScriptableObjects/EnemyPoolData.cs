using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Attributes;
using _Main.Scripts.DevelopmentUtilities;
using Enemies;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyPoolData", menuName = "Main/Pools/EnemyPoolData", order = 0)]
    public class EnemyPoolData : ScriptableObject
    {
        [SerializeField] private List<EnemyModel> allEnemiesPrefabs;
        [SerializeField] private List<float> allEnemiesChances;
        
        private RouletteWheel<EnemyModel> m_rouletteWheel;
        
        public EnemyModel GetRandomEnemyPrefabFromPool()
        {
            m_rouletteWheel ??= new RouletteWheel<EnemyModel>(allEnemiesPrefabs, allEnemiesChances);

            return m_rouletteWheel.RunWithCached();
        }
        
#if UNITY_EDITOR
        [Header("Only Editor")]
        [ReadOnlyInspector, SerializeField] public List<float> chancePercentage = new();
        

        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = allEnemiesPrefabs.Distinct().ToList();
            allEnemiesPrefabs = l_newList;

            if (allEnemiesPrefabs.Count != allEnemiesChances.Count)
            {
                while (allEnemiesPrefabs.Count > allEnemiesChances.Count)
                {
                    allEnemiesChances.Add(0);
                }

                while (allEnemiesPrefabs.Count < allEnemiesChances.Count)
                {
                    allEnemiesChances.RemoveAt(allEnemiesChances.Count - 1);
                }
            }

            CheckItemPercentage();
        }

        [ContextMenu("Show item percentage")]
        public void CheckItemPercentage()
        {
            var l_totalChance = allEnemiesChances.Sum();

            chancePercentage.Clear();

            for (var l_i = 0; l_i < allEnemiesChances.Count; l_i++)
            {
                chancePercentage.Add((allEnemiesChances[l_i] / l_totalChance) * 100);
            }
        }

        [ContextMenu("Clear percentage")]
        public void ClearPercentage()
        {
            chancePercentage.Clear();
        }
#endif
    }
}
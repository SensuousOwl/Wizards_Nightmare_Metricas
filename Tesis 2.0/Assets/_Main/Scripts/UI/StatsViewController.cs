
using System;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.UI
{
    public class StatsViewController : MonoBehaviour
    {
        [SerializeField] private TMP_Text movementSpeed_Text;
        [SerializeField] private TMP_Text fireRate_Text;
        [SerializeField] private TMP_Text fireRange_Text;
        [SerializeField] private TMP_Text damage_Text;
        [SerializeField] private TMP_Text critDama_Text;
        [SerializeField] private TMP_Text critChance_Text;
        [SerializeField] private TMP_Text projectileSpeed_Text;
        private static IStatsService StatsController => ServiceLocator.Get<IStatsService>();

        private void Start()
        {
            var l_dic = StatsController.GetAllStatData();

            
            foreach (var l_data in l_dic)
            {
                ChangeStatValue(l_data.Key, l_data.Value);
            }
        }

        private void Awake()
        {
            StatsController.OnChangeStatValue += ChangeStatValue;
        }

        private void OnDestroy()
        {
            StatsController.OnChangeStatValue -= ChangeStatValue;
        }

        private void ChangeStatValue(StatsId p_statId, float p_value)
        {
            var l_stringValue = String.Format("{0 : 0.00}", p_value);
            switch (p_statId)
            {
                case StatsId.MovementSpeed :
                    movementSpeed_Text.text = l_stringValue;
                    break;
                case StatsId.FireRate :
                    fireRate_Text.text = l_stringValue;
                    break;
                case StatsId.Range :
                    fireRange_Text.text = l_stringValue;
                    break;
                case StatsId.Damage :
                    damage_Text.text = l_stringValue;
                    break;
                case StatsId.CriticalDamageMult :
                    critDama_Text.text = l_stringValue;
                    break;
                case StatsId.CriticalChance :
                    critChance_Text.text = l_stringValue;
                    break;
                case StatsId.ProjectileSpeed :
                    projectileSpeed_Text.text = l_stringValue;
                    break;
            }
        }
    }
}
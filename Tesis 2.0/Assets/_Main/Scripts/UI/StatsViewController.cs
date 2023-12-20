
using System;
using _Main.Scripts.PlayerScripts;
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
        public IStatsService StatsController => ServiceLocator.Get<IStatsService>();

        private void Start()
        {
            var dic = StatsController.GetAllStatData();

            foreach (var data in dic)
            {
                ChangeStatValue(data.Key, data.Value);
            }
        }

        private void OnEnable()
        {
            StatsController.OnChangeStatValue += ChangeStatValue;
        }

        private void OnDisable()
        {
            StatsController.OnChangeStatValue -= ChangeStatValue;
        }

        private void ChangeStatValue(StatsId statId, float value)
        {
            var stringValue = String.Format("{0 : 0.00}", value);
            switch (statId)
            {
                case StatsId.MovementSpeed :
                    movementSpeed_Text.text = stringValue;
                    break;
                case StatsId.FireRate :
                    fireRate_Text.text = stringValue;
                    break;
                case StatsId.Range :
                    fireRange_Text.text = stringValue;
                    break;
                case StatsId.Damage :
                    damage_Text.text = stringValue;
                    break;
                case StatsId.CriticalDamageMult :
                    critDama_Text.text = stringValue;
                    break;
                case StatsId.CriticalChance :
                    critChance_Text.text = stringValue;
                    break;
                case StatsId.ProjectileSpeed :
                    projectileSpeed_Text.text = stringValue;
                    break;
            }
        }
    }
}
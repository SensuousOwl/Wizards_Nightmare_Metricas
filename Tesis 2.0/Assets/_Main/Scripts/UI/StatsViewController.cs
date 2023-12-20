
using System;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class StatsViewController : MonoBehaviour
    {
        [SerializeField] private TMP_Text hp_Text;
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
            switch (statId)
            {
                case StatsId.MovementSpeed :
                    movementSpeed_Text.text = value.ToString();
                    break;
                case StatsId.FireRate :
                    fireRate_Text.text = value.ToString();
                    break;
                case StatsId.Range :
                    fireRange_Text.text = value.ToString();
                    break;
                case StatsId.Damage :
                    damage_Text.text = value.ToString();
                    break;
                case StatsId.CriticalDamageMult :
                    critDama_Text.text = value.ToString();
                    break;
                case StatsId.CriticalChance :
                    critChance_Text.text = value.ToString();
                    break;
                case StatsId.ProjectileSpeed :
                    projectileSpeed_Text.text = value.ToString();
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using _Main.Scripts.Managers;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.UpgradePoolServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI.Menus
{
    public class UpgradeScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject screenObj;
        [SerializeField] private int upgradesCount;
        [SerializeField] private List<TMP_Text> namesTxt = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> descriptionTxt = new List<TMP_Text>();
        [SerializeField] private List<Image> buttonImages = new List<Image>();
        [SerializeField] private List<Image> effectImages = new List<Image>();

        public IUpgradePoolService UpgradePoolService => ServiceLocator.Get<IUpgradePoolService>();
        
        private List<UpgradeData> m_currUpgradeDatas = new List<UpgradeData>();
        private List<UpgradeData> m_previusUpgradeDatas = new List<UpgradeData>();

        private void Awake()
        {
            screenObj.SetActive(false);
        }

        

        private void OnPause(bool obj)
        {
            throw new NotImplementedException();
        }

        public void ActivateUpgradeScreen()
        {
            if (screenObj == null)
            {
                Debug.LogError("screenObj ha sido destruido o no está asignado.");
                return;
            }

            for (int i = 0; i < upgradesCount; i++)
            {
                if (UpgradePoolService == null)
                {
                    Debug.LogError("UpgradePoolService no está disponible.");
                    return;
                }

                var upgradeData = UpgradePoolService.GetRandomUnlockedUpgradeFromPool(m_previusUpgradeDatas);
                if (upgradeData == null)
                {
                    Debug.LogWarning($"No se pudo obtener un upgrade para la posición {i}");
                    continue;
                }

                m_currUpgradeDatas.Add(upgradeData);
                m_previusUpgradeDatas.Add(upgradeData);

                if (i < namesTxt.Count && namesTxt[i] != null)
                    namesTxt[i].text = upgradeData.Name;

                if (i < descriptionTxt.Count && descriptionTxt[i] != null)
                    descriptionTxt[i].text = upgradeData.Description;

                if (i < buttonImages.Count && buttonImages[i] != null)
                    buttonImages[i].sprite = upgradeData.BorderSprite;

                if (i < effectImages.Count && effectImages[i] != null)
                    effectImages[i].sprite = upgradeData.EffectSprite;
            }

            screenObj.SetActive(true);
            PauseManager.Instance.SetPauseUpgrade(true);
        }


        public void OnPressedButton(int p_buttonId)
        {
            m_currUpgradeDatas[p_buttonId].ApplyEffects();
            m_previusUpgradeDatas.Clear();
            m_currUpgradeDatas.Clear();
            PauseManager.Instance.SetPauseUpgrade(false);
            screenObj.SetActive(false);
        }
    }
}
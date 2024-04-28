using System;
using System.Collections.Generic;
using _Main.Scripts.Managers;
using _Main.Scripts.PlayerScripts;
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

        private PlayerModel m_model;

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
            for (int i = 0; i < upgradesCount; i++)
            {
                m_currUpgradeDatas.Add(UpgradePoolService.GetRandomUnlockedUpgradeFromPool(m_previusUpgradeDatas)); 
                m_previusUpgradeDatas.Add(m_currUpgradeDatas[i]);
                
                namesTxt[i].text = m_currUpgradeDatas[i].Name;
                descriptionTxt[i].text = m_currUpgradeDatas[i].Description;

                buttonImages[i].sprite = m_currUpgradeDatas[i].BorderSprite;
                effectImages[i].sprite = m_currUpgradeDatas[i].EffectSprite;
            }
            
            screenObj.SetActive(true);
            PauseManager.Instance.SetPauseUpgrade(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnPressedButton(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnPressedButton(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnPressedButton(2);
            }
        }

        public void OnPressedButton(int p_buttonId)
        {
            if (m_model == default)
                return;
            m_currUpgradeDatas[p_buttonId].ApplyEffects(m_model);
            m_previusUpgradeDatas.Clear();
            m_currUpgradeDatas.Clear();
            PauseManager.Instance.SetPauseUpgrade(false);
            screenObj.SetActive(false);
        }

        public void SetPlayerModel(PlayerModel p_model) => m_model = p_model;

    }
}
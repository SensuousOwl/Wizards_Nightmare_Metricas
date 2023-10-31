using System;
using System.Collections.Generic;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeScreenController : MonoBehaviour
    {
        [SerializeField] private UpgradeDataPool pool;
        [SerializeField] private GameObject screenObj;
        [SerializeField] private int upgradesCount;
        [SerializeField] private List<TMP_Text> namesTxt = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> descriptionTxt = new List<TMP_Text>();

        private List<UpgradeData> m_currUpgradeDatas = new List<UpgradeData>();
        //private List<UpgradeData> m_previusUpgradeDatas = new List<UpgradeData>();


        private void Awake()
        {
            ActivateUpgradeScreen();
        }

        public void ActivateUpgradeScreen()
        {
            m_currUpgradeDatas.Clear();
            for (int i = 0; i < upgradesCount; i++)
            {
                m_currUpgradeDatas[i] = pool.GetRandomUpgradeFromPool(m_currUpgradeDatas);
                namesTxt[i].text = m_currUpgradeDatas[i].Name;
                descriptionTxt[i].text = m_currUpgradeDatas[i].Description;
            }
            
            
            
            screenObj.SetActive(true);
        }

        public void OnPressedButton(int p_buttonId)
        {
            m_currUpgradeDatas[p_buttonId].ApplyEffects(FindObjectOfType<PlayerModel>());
            ActivateUpgradeScreen();
        }



    }
}
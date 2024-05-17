using System.Collections.Generic;
using _Main.Scripts.Managers;
using _Main.Scripts.ScriptableObjects.UpgradesSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI.HUB
{
    public class UnlockUpgradeShowUI : MonoBehaviour
    {
        [SerializeField] private GameObject screenObj;
        [SerializeField] private List<TMP_Text> namesTxt = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> descriptionTxt = new List<TMP_Text>();
        [SerializeField] private List<Image> upgradeImages = new List<Image>();
        [SerializeField] private List<Image> effectImages = new List<Image>();

        private void Start()
        {
            screenObj.SetActive(false);
        }

        public void ActivateUnlockScreen(List<UpgradeData> p_upgradeData)
        {
            for (int i = 0; i < p_upgradeData.Count; i++)
            {
                var l_currData = p_upgradeData[i];
                namesTxt[i].text = l_currData.Name;
                descriptionTxt[i].text = l_currData.Description;
                upgradeImages[i].sprite = l_currData.BorderSprite;
                effectImages[i].sprite = l_currData.EffectSprite;
            }
            
            
            screenObj.SetActive(true);
            PauseManager.Instance.SetPauseUpgrade(true);
        }


        public void DeactivateScreen()
        {
            screenObj.SetActive(false);
            PauseManager.Instance.SetPauseUpgrade(false);
        }
    }
}
using System;
using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using _Main.Scripts.Services.MicroServices.UserDataService;
using _Main.Scripts.Services.UpgradePoolServices;
using _Main.Scripts.UI.HUB;
using UnityEngine;

namespace _Main.Scripts.Interactables
{
    public class UnlockUpgradesChest : MonoBehaviour, IInteract
    {
        [SerializeField] private int UpgradeCost;
        [SerializeField] private int UnlockUpgradeAmmount;
        [SerializeField] private UnlockUpgradeShowUI UpgradePanel;
        [SerializeField] private GameObject interactVisual;
        
        
        private static IUpgradePoolService UpgradePoolService => ServiceLocator.Get<IUpgradePoolService>();
        private static ICurrencyService CurrencyService => ServiceLocator.Get<ICurrencyService>();
        public void Interact(PlayerModel p_model)
        {
            if (CurrencyService.GetCurrentGs() >= UpgradeCost)
            {
                var l_upgradeList = UpgradePoolService.GetRandomUpgradesAndUnlock(UnlockUpgradeAmmount);
                CurrencyService.AddGs(-UpgradeCost);
                UpgradePanel.ActivateUnlockScreen(l_upgradeList);
            }
        }
        public void ShowCanvasUI(bool p_b)
        {
            interactVisual.SetActive(p_b);
        }

        private void OnTriggerEnter(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(true);
            }
        }

        private void OnTriggerExit(Collider p_other)
        {
            if (p_other.CompareTag("Player"))
            {
                ShowCanvasUI(false);
            }
        }
    }
}
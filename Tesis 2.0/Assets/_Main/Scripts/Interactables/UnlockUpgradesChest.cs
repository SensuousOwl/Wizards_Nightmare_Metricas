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
        
        private IUpgradePoolService m_upgradePoolService => ServiceLocator.Get<IUpgradePoolService>();
        private ICurrencyService m_currencyService => ServiceLocator.Get<ICurrencyService>();
        public void Interact(PlayerModel p_model)
        {
            if (m_currencyService.GetCurrentGs() >= UpgradeCost)
            {
                var l_upgradeList = m_upgradePoolService.GetRandomUpgradesAndUnlock(UnlockUpgradeAmmount);
                m_currencyService.AddGs(-UpgradeCost);
                UpgradePanel.ActivateUnlockScreen(l_upgradeList);
            }
        }
    }
}
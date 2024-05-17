using _Main.Scripts.Services;
using _Main.Scripts.Services.CurrencyServices;
using _Main.Scripts.Services.MicroServices.InventoryService;
using _Main.Scripts.UI.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class InventoryPanel : BasePanel
    {
        [SerializeField] private Button goBackInventoryButton;

        [SerializeField] private string nameDefaultNotItem;
        [SerializeField, Multiline] private string descriptionDefaultNotItem;
        [SerializeField] private string rarityDefaultNotItem;

        [SerializeField] private TMP_Text moneyCountText;
        [SerializeField] private TMP_Text nameActiveText;
        [SerializeField] private TMP_Text namePassiveText;
        [SerializeField] private TMP_Text descriptionActiveText;
        [SerializeField] private TMP_Text descriptionPassiveText;
        [SerializeField] private TMP_Text rarityActiveText;
        [SerializeField] private TMP_Text rarityPassiveText;
        [SerializeField] private Image imageActiveText;
        [SerializeField] private Image imagePassiveText;

        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();
        private static ICurrencyService CurrencyService => ServiceLocator.Get<ICurrencyService>();

        public void Initialize()
        {
            Close();
            goBackInventoryButton.onClick.AddListener(CloseInventoryPanel);
        }

        private void CloseInventoryPanel()
        {
            Close();
        }

        public override void Open()
        {
            moneyCountText.text = CurrencyService.GetCurrentGs().ToString();
            UpdateActiveItem();
            UpdatePassiveItem();
            base.Open();
        }

        private void UpdateActiveItem()
        {
            var l_item = InventoryService.GetActiveItem();

            if (l_item == default)
            {
                nameActiveText.text = nameDefaultNotItem;
                descriptionActiveText.text = descriptionDefaultNotItem;
                rarityActiveText.text = rarityDefaultNotItem;
                imageActiveText.enabled = false;
                return;
            }

            nameActiveText.text = l_item.Name;
            descriptionActiveText.text = l_item.Description;
            rarityActiveText.text = l_item.ItemRarity.ToString();
            imageActiveText.sprite = l_item.Sprite;
            imageActiveText.enabled = true;
        }

        private void UpdatePassiveItem()
        {
            var l_item = InventoryService.GetPassiveItem();

            if (l_item == default)
            {
                namePassiveText.text = nameDefaultNotItem;
                descriptionPassiveText.text = descriptionDefaultNotItem;
                rarityPassiveText.text = rarityDefaultNotItem;
                imagePassiveText.enabled = false;
                return;
            }

            namePassiveText.text = l_item.Name;
            descriptionPassiveText.text = l_item.Description;
            rarityPassiveText.text = l_item.ItemRarity.ToString();
            imagePassiveText.enabled = true;
            imagePassiveText.sprite = l_item.Sprite;
        }
    }
}
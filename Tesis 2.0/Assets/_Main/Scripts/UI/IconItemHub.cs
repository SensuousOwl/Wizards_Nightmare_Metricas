using System;
using _Main.Scripts.ItemsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
{
    public class IconItemHub : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ItemType itemType;

        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();
        
        private void Awake()
        {
            image.gameObject.SetActive(false);

            switch (itemType)
            {
                case ItemType.Instant:
                    break;
                case ItemType.Passive:
                    var l_itemPassive = InventoryService.GetPassiveItem();
                    if (l_itemPassive == default)
                        break;
                    image.sprite = l_itemPassive.Sprite;
                    image.gameObject.SetActive(true);
                    InventoryService.OnUpdatePassiveItem += UpdateItemHandler;
                    break;
                case ItemType.Active:
                    var l_itemActive = InventoryService.GetActiveItem();
                    if (l_itemActive == default)
                        break;
                    image.sprite = l_itemActive.Sprite;
                    image.gameObject.SetActive(true);
                    InventoryService.OnUpdateActiveItem += UpdateItemHandler;
                    InventoryService.OnCooldownActiveItem += InventoryServiceOnOnCooldownActiveItem;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            switch (itemType)
            {
                case ItemType.Instant:
                    break;
                case ItemType.Passive:
                    InventoryService.OnUpdatePassiveItem -= UpdateItemHandler;
                    break;
                case ItemType.Active:
                    InventoryService.OnUpdateActiveItem -= UpdateItemHandler;
                    InventoryService.OnCooldownActiveItem -= InventoryServiceOnOnCooldownActiveItem;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InventoryServiceOnOnCooldownActiveItem(bool p_value)
        {
            image.color = p_value ? Color.grey : Color.white;
        }

        private void UpdateItemHandler()
        {
            switch (itemType)
            {
                case ItemType.Instant:
                    break;
                case ItemType.Passive:
                    var l_itemPassive = InventoryService.GetPassiveItem();
                    if (l_itemPassive == default)
                    {
                        image.gameObject.SetActive(false);
                        image.sprite = default;
                        return;
                    }
                    image.sprite = l_itemPassive.Sprite;
                    image.gameObject.SetActive(true);
                    break;
                case ItemType.Active:
                    var l_itemActive = InventoryService.GetActiveItem();
                    if (l_itemActive == default)
                    {
                        image.gameObject.SetActive(false);
                        image.sprite = default;
                        return;
                    }
                    image.sprite = l_itemActive.Sprite;
                    image.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
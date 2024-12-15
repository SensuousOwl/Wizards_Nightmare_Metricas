using _Main.Scripts.Interfaces;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using Unity.Services.Analytics;
using UnityEngine;
using System.Collections.Generic;

namespace _Main.Scripts.ItemsSystem
{
    public class BaseItem : MonoBehaviour, IInteract
    {
        [SerializeField] private ItemData data;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();

        private void Awake()
        {
            if (data != default)
                spriteRenderer.sprite = data.Sprite;
        }

        public ItemData GetItemData() => data;

        public void SetItemData(ItemData p_newData)
        {
            data = p_newData;
            spriteRenderer.sprite = data.Sprite;
        }

        public void Interact()
        {
            InventoryService.AddItem(data, transform.position);

            // Incrementar el contador global
            ExperienceController.Instance.IncrementItemPickupCount();

            // Enviar evento a Unity Analytics
            SendItemPickupAnalytics(data);

            Destroy(gameObject);
        }

        private void SendItemPickupAnalytics(ItemData itemData)
        {
            if (itemData == null)
                return;

            // Enviar evento a Unity Analytics
            AnalyticsService.Instance.CustomData("Item_Picked_Up", new Dictionary<string, object>
            {
                { "ItemTypee", itemData.ItemType.ToString() },
                { "ItemRarity", itemData.ItemRarity.ToString() }
            });

            // Enviar los datos inmediatamente
            AnalyticsService.Instance.Flush();

            Debug.Log($"Evento enviado: ItemTypee={itemData.ItemType}, ItemRarity={itemData.ItemRarity}");
        }

        public void ShowCanvasUI(bool p_b)
        {
        }
    }
}
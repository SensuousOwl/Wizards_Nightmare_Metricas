using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using UnityEngine;

namespace _Main.Scripts.ItemsSystem
{
    public class BaseItem : MonoBehaviour, IInteract
    {
        [SerializeField] private ItemData data;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject interactVisual;
        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();

        private void Awake()
        {
            spriteRenderer.sprite = data.Sprite;
        }

        public ItemData GetItemData() => data;

        public void SetItemData(ItemData p_newData)
        {
            data = p_newData;
            spriteRenderer.sprite = data.Sprite;
        }

        public void Interact(PlayerModel p_model)
        {
            InventoryService.AddItem(data);
            Destroy(gameObject);
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
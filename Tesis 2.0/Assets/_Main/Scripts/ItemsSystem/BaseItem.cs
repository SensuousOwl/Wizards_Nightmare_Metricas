using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Main.Scripts.ItemsSystem
{
    public class BaseItem : MonoBehaviour, IInteract
    {
        [SerializeField] private ItemData data;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer.sprite = data.Sprite;
        }

        public void Interact(PlayerModel p_model)
        {
            p_model.Inventory.AddItem(data);
            Destroy(gameObject);
        }
    }
}
using System;
using _Main.Scripts.ScriptableObjects.ItemsSystem;

namespace _Main.Scripts.Services.MicroServices.InventoryService
{
    public interface IInventoryService : IGameService
    {
        event Action OnUpdateActiveItem;
        event Action OnUpdatePassiveItem;
        event Action<bool> OnCooldownActiveItem;
        
        ItemData GetActiveItem();
        ItemData GetPassiveItem();
        bool HasActiveItem();
        bool HasPassiveItem();
        public void SetActiveItem(ItemData p_newItem);
        public void SetPassiveItem(ItemData p_newItem);
        public void RemoveActiveItem();
        public void RemovePassiveItem();
        void AddItem(ItemData p_itemData);
        void UseActiveItem();
    }
}
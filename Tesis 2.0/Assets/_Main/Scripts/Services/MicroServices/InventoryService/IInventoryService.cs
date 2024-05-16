using System;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using UnityEngine;

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
        public void DropActiveItem(Vector3 p_position);
        public void DropPassiveItem(Vector3 p_position);
        void AddItem(ItemData p_itemData);
        void UseActiveItem();
    }
}
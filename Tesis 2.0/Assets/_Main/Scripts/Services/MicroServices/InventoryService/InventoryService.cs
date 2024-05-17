using System;
using System.Collections;
using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.ItemsSystem;
using _Main.Scripts.ScriptableObjects.ItemsSystem;
using _Main.Scripts.Services.MicroServices.SpawnItemsService;
using _Main.Scripts.Services.Stats;
using UnityEngine;

namespace _Main.Scripts.Services.MicroServices.InventoryService
{
    public class InventoryService : IInventoryService
    {
        private bool m_activeItemCooldown;
        
        private ItemData m_activeItem;
        private int m_useActiveCount;
        private ItemData m_passiveItem;

        public event Action OnUpdateActiveItem;
        public event Action OnUpdatePassiveItem;
        public event Action<bool> OnCooldownActiveItem;

        private static IStatsService StatsService => ServiceLocator.Get<IStatsService>();
        private static ISpawnItemsService SpawnItemsService => ServiceLocator.Get<ISpawnItemsService>();

        public void Initialize()
        {
            m_activeItemCooldown = false;
            m_useActiveCount = 0;
            
            m_activeItem = default;
            m_passiveItem = default;
        }
        public ItemData GetActiveItem() => m_activeItem;
        public ItemData GetPassiveItem() => m_passiveItem;
        public bool HasActiveItem() => m_activeItem != default;
        public bool HasPassiveItem() => m_passiveItem != default;

        public void UseActiveItem()
        {
            if (m_activeItem == default)
                return;
            
            if (m_activeItemCooldown)
                return;
            m_activeItem.ItemActiveEffect.UseItem();
            m_useActiveCount--;
            
            
            if (m_activeItem.IsItemCooldown)
            {
                var l_playerModel = PlayerModel.Local;
                l_playerModel.StartCoroutine(ActiveCooldownCoroutine());
                return;
            }
            if (m_useActiveCount > 0)
                return;
            
            RemoveActiveItem();
        }

        public void SetActiveItem(ItemData p_newItem)
        {
            if (p_newItem.ItemType != ItemType.Active)
                return;
            
            RemoveActiveItem();
            
            m_activeItem = p_newItem;
            m_useActiveCount = m_activeItem.UseCount;
            OnUpdateActiveItem?.Invoke();
        }
        
        public void SetPassiveItem(ItemData p_newItem)
        {
            if (p_newItem.ItemType != ItemType.Passive)
                return;
            
            RemovePassiveItem();
            
            m_passiveItem = p_newItem;
            m_passiveItem.ItemPassiveEffect.Activate();
            OnUpdatePassiveItem?.Invoke();
        }

        public void RemoveActiveItem()
        {
            if (m_activeItem == default)
                return;

            m_activeItem = default;
            OnUpdateActiveItem?.Invoke();
        }
        
        public void RemovePassiveItem()
        {
            if (m_passiveItem == default)
                return;

            m_passiveItem.ItemPassiveEffect.Deactivate();
            m_passiveItem = default;
            OnUpdatePassiveItem?.Invoke();
        }
        
        
        public void DropActiveItem(Vector3 p_position)
        {
            if (m_activeItem == default)
                return;
            
            SpawnItemsService.SpawnItem(m_activeItem, p_position);
            RemoveActiveItem();
        }
        
        public void DropPassiveItem(Vector3 p_position)
        {
            if (m_passiveItem == default)
                return;
            
            SpawnItemsService.SpawnItem(m_passiveItem, p_position);
            RemovePassiveItem();
        }

        private IEnumerator ActiveCooldownCoroutine()
        {
            m_useActiveCount = m_activeItem.UseCount;
            m_activeItemCooldown = true;
            OnCooldownActiveItem?.Invoke(m_activeItemCooldown);
            var l_subtractCooldown = m_activeItem.TimeToCooldownInSeconds *
                                      (StatsService.GetStatById(StatsId.SubtractItemActiveCooldown) / 100);
            var l_cooldown = m_activeItem.TimeToCooldownInSeconds - l_subtractCooldown;
            yield return new WaitForSeconds(l_cooldown);
            m_activeItemCooldown = false;
            OnCooldownActiveItem?.Invoke(m_activeItemCooldown);
        }
        
        public void AddItem(ItemData p_itemData, Vector3 p_position)
        {
            switch (p_itemData.ItemType)
            {
                case ItemType.Instant:
                    p_itemData.ItemActiveEffect.UseItem();
                    return;
                case ItemType.Passive:
                    DropPassiveItem(p_position);
                    SetPassiveItem(p_itemData);
                    return;
                case ItemType.Active:
                    DropActiveItem(p_position);
                    SetActiveItem(p_itemData);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
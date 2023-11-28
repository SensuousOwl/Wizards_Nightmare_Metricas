using _Main.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Main.Scripts.PlayerScripts
{
    public class Inventory
    {
        private readonly PlayerModel m_model;
        private ItemData m_item;

        public Inventory(PlayerModel p_model)
        {
            m_model = p_model;
        }

        public void UseItem()
        {
            if (m_item == default)
                return;
            
            if (m_item.UpgradeEffect != default)
                m_item.UpgradeEffect.ApplyEffect(m_model, m_item.UpgradeValuePercentage);

            if (m_item.SpawnObject != default)
                Object.Instantiate(m_item.SpawnObject, m_model.transform.position, m_item.SpawnObject.transform.rotation);

            m_item = default;
        }

        public void AddItem(ItemData p_itemData)
        {
            m_item = p_itemData;
        }
    }
}
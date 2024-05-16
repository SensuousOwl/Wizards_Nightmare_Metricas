using UnityEngine;

namespace _Main.Scripts.ItemsSystem
{
    public class ItemVisualController : MonoBehaviour
    {
        [SerializeField] private GameObject interactVisual;
        [SerializeField] private BaseItem baseItem;

        private void Awake()
        {
            ShowCanvasUI(false);
        }

        private void ShowCanvasUI(bool p_b)
        {
            interactVisual.SetActive(p_b);
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (!p_other.CompareTag("Player"))
                return;
            if (baseItem.GetItemData().ItemType == ItemType.Instant)
            {
                baseItem.Interact();
                return;
            }
            
            ShowCanvasUI(true);
        }

        private void OnTriggerExit2D(Collider2D p_other)
        {
            if (!p_other.CompareTag("Player"))
                return;
            if (baseItem.GetItemData().ItemType == ItemType.Instant)
            {
                return;
            }
            
            ShowCanvasUI(false);
        }
    }
}

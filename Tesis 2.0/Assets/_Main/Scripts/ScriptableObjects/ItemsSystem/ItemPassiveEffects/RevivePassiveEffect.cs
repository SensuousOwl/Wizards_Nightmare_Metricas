using _Main.Scripts.Entities.PlayerScripts.MVC;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects.ItemsSystem.ItemPassiveEffects
{
    [CreateAssetMenu(menuName = "Main/Items/Effects/Passive/Revive")]
    public class RevivePassiveEffect : ItemPassiveEffect
    {
        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();
        public override void Activate()
        {
            PlayerModel.Local.SetRevive(true);
            PlayerModel.Local.OnRevive += OnReviveHandler;
        }

        private static void OnReviveHandler()
        {
            InventoryService.RemovePassiveItem();
        }

        public override void Deactivate()
        {
            PlayerModel.Local.SetRevive(false);
            PlayerModel.Local.OnRevive -= OnReviveHandler;
        }
    }
}
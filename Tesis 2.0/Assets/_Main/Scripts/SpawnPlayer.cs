using _Main.Scripts.Managers;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.UI;
using _Main.Scripts.UI.Menus;
using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerPrefab;
        [SerializeField] private UpgradeScreenController upgradeScreenController;

        private void Awake()
        {
            var l_player = Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            InputManager.Instance.ChangeActionMap("Default-Keyboard");
            upgradeScreenController.SetPlayerModel(l_player);

            
            Destroy(gameObject);
        }
    }
}
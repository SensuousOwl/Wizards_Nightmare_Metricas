using _Main.Scripts.Managers;
using _Main.Scripts.PlayerScripts;
using _Main.Scripts.UI;
using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerPrefab;
        [SerializeField] private UpgradeScreenController upgradeScreenController;

        private void Start()
        {
            var l_player = Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            upgradeScreenController.SetPlayerModel(l_player);

            
            Destroy(gameObject);
        }
    }
}
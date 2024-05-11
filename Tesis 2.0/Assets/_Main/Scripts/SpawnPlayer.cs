using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerPrefab;

        private void Start()
        {
            Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            InputManager.Instance.ChangeActionMap("Default-Keyboard");

            
            Destroy(gameObject);
        }
    }
}
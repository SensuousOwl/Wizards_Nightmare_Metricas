using UnityEngine;

namespace _Main.Scripts
{
    public class SpawnOnStart : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;

        private void Start()
        {
            Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
            Destroy(gameObject);
        }
    }
}
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;


        public PlayerModel PlayerModel { get; private set; }
        private void Awake()
        {
            if(Instance != default)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public void SetPlayerModel(PlayerModel p_model) => PlayerModel = p_model;
    }
}
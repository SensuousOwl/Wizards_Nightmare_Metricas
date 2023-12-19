namespace _Main.Scripts.Audio
{
    public class GlobalItemAudioPoolPlayer : SfxAudioPoolPlayer
    {
        public static GlobalItemAudioPoolPlayer Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
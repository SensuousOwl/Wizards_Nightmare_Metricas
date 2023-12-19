using System;
using UnityEngine;

namespace _Main.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private float m_timerToDeactivate;

        public event Action<AudioSourceController> OnDeactivateEvent;
        
        private void Update()
        {
            if (m_timerToDeactivate > Time.time)
                return;

            audioSource.Stop();
            OnDeactivateEvent?.Invoke(this);
        }

        public void PlayOneShot(AudioClip p_audioClip, float p_volumeScale = 1f)
        {
            gameObject.SetActive(true);
            m_timerToDeactivate = Time.time + p_audioClip.length;
            audioSource.volume = p_volumeScale;
            audioSource.PlayOneShot(p_audioClip, p_volumeScale);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == default)
                Debug.LogError("AudioSource Not Found");
        }
#endif
    }
}
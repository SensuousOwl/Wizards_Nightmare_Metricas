using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using UnityEngine;

namespace _Main.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbienceSfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] ambienceClips;
        [SerializeField] private float preemptiveThreshold = 2f;
        private AudioSource m_audioSource;
        private float m_clipTimeout;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            ResetClip();
        }

        private void ResetClip()
        {
            var l_currClip = ambienceClips.GetRandomElement();
            m_clipTimeout = Time.time + l_currClip.length - preemptiveThreshold;
            m_audioSource.PlayOneShot(l_currClip);
        }

        private void Update()
        {
            if (Time.time < m_clipTimeout)
                return;
            ResetClip();
        }
    }
}
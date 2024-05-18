using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.ScriptableObjects.Audio;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.EventsServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace _Main.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private LevelBGMAudioData audioData;
        [SerializeField] private float transitionTime;
        [SerializeField] private AudioSourceController audioSourcePrefab;
        [SerializeField] private AudioSource noBattleAudioSource, inBattleAudioSource, bossBattleAudioSource;

        private BGMType m_currentlyPlayingSource;
        private readonly Dictionary<BGMType, AudioSource> m_bgmTypeToAudioSource = new();

        private PoolGeneric<AudioSourceController> m_audioSourceControllerPool;

        private static IEventService EventService => ServiceLocator.Get<IEventService>();

        public AudioMixer mixer;

        private void Awake()
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            SetMasterVolume(masterVolume);
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SetMasterVolume(musicVolume);
            float SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            SetMasterVolume(SFXVolume);
            Initialize();

            
            EventService.AddListener<PlayEffectSound>(OnPlayEffectSoundHandler);
        }

        private void OnPlayEffectSoundHandler(PlayEffectSound p_data)
        {
            var l_audioSource = m_audioSourceControllerPool.GetorCreate();
            l_audioSource.PlayOneShot(p_data.AudioClip, p_data.Volume);
            l_audioSource.OnDeactivateEvent += OnDeactivateEventAudioSourceControllerHandler;
        }

        private void OnDeactivateEventAudioSourceControllerHandler(AudioSourceController p_audioSourceController)
        {
            p_audioSourceController.OnDeactivateEvent -= OnDeactivateEventAudioSourceControllerHandler;
            p_audioSourceController.gameObject.SetActive(false);
            m_audioSourceControllerPool.AddPool(p_audioSourceController);
        }

        private void OnDisable()
        {
            EventService.RemoveListener<PlayEffectSound>(OnPlayEffectSoundHandler);
        }
        
        void SetVolumeFromPrefs(AudioMixer mixer, string parameterName, string prefsKey, float defaultValue)
        {
            float volume = PlayerPrefs.GetFloat(prefsKey, defaultValue);
            mixer.SetFloat(parameterName, LinearToDecibel(volume));
        }
        
        public float LinearToDecibel(float linear)
        {
            return linear != 0 ? Mathf.Log10(linear) * 20 : -80f;
        }

        private void Initialize()
        {
            
            m_audioSourceControllerPool = new PoolGeneric<AudioSourceController>(audioSourcePrefab, transform);
            
            m_bgmTypeToAudioSource.Add(BGMType.NoBattle, noBattleAudioSource);
            m_bgmTypeToAudioSource.Add(BGMType.InBattle, inBattleAudioSource);
            m_bgmTypeToAudioSource.Add(BGMType.BossBattle, bossBattleAudioSource);
            ResumeNormalGameplayLoops();
            SetVolumeFromPrefs(mixer, "MasterVolume", "MasterVolume", 0.75f);
            SetVolumeFromPrefs(mixer, "MusicVolume", "MusicVolume", 0.75f);
            SetVolumeFromPrefs(mixer, "SFXVolume", "SFXVolume", 0.75f);
        }

        private void CrossFade(BGMType p_newBGMType)
        {
            if (p_newBGMType == BGMType.BossBattle)
            {
                ToBossBattle();
                return;
            }

            var l_playingAudioSource = m_bgmTypeToAudioSource[m_currentlyPlayingSource];
            var l_newAudioSource = m_bgmTypeToAudioSource[p_newBGMType];
            m_currentlyPlayingSource = p_newBGMType;
            l_playingAudioSource.DOFade(0, transitionTime);
            l_newAudioSource.DOFade(1, transitionTime);
        }

        private void AfterBossHandler()
        {
            CrossFade(BGMType.NoBattle);
        }

        private void ToBossBattle()
        {
            StopCurrentAudio();
            m_currentlyPlayingSource = BGMType.BossBattle;
            var l_playingAudioSource = m_bgmTypeToAudioSource[m_currentlyPlayingSource];
            l_playingAudioSource.PlayThenToLoop(audioData.BossBattleIntro, audioData.BossBattleBGM);
        }

        private void StopCurrentAudio()
        {
            var l_playingAudioSource = m_bgmTypeToAudioSource[m_currentlyPlayingSource];
            l_playingAudioSource.Stop();
        }

        private void StopAllAudio()
        {
            foreach (var l_bgmTypeSource in m_bgmTypeToAudioSource)
            {
                l_bgmTypeSource.Value.Stop();
            }
        }

        private void ResumeNormalGameplayLoops()
        {
            m_currentlyPlayingSource = BGMType.NoBattle;
            noBattleAudioSource.PlayThenToLoop(audioData.NoBattleIntro, audioData.NoBattleBGM);
            inBattleAudioSource.Stop();
        }

        private void BattleGameplayLoops()
        {
            m_currentlyPlayingSource = BGMType.InBattle;
            noBattleAudioSource.PlayThenToLoop(audioData.NoBattleIntro, audioData.NoBattleBGM);
            inBattleAudioSource.Stop();
        }
        
        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }

#if UNITY_EDITOR

        [Header("EDITOR ONLY")] 
        [SerializeField] private BGMType bgmToFadeTest;

        [ContextMenu("Test fades")]
        private void TestFade()
        {
            CrossFade(bgmToFadeTest);
        }
#endif
    }
    
    public struct PlayEffectSound : ICustomEventData
    {
        public AudioClip AudioClip { get; }
        public float Volume { get; }
        
        public PlayEffectSound(AudioClip p_audioClip, float p_v=1)
        {
            AudioClip = p_audioClip;
            Volume = p_v;
        }
    }
}
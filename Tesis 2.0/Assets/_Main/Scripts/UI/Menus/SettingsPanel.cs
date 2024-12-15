using System.Collections.Generic;
using _Main.Scripts.Audio;
using _Main.Scripts.Managers;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Main.Scripts.UI.Menus
{
    public class SettingsPanel : BasePanel
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle hudAlwaysActiveToggle;
        [SerializeField] private Toggle hudHiddenToggle;
        [SerializeField] private Slider alphaHUDSlider;
        [SerializeField] private BasePanel controlsScreen;
        [SerializeField] private Button controlsScreenButton;
        [SerializeField] private Button goBackControlButton;
        [SerializeField] private Button goBackScreenButton;
        [SerializeField] private CanvasGroup hudCanvasGroup;

        private const string HUD_ALWAYS_ACTIVE_KEY = "HUDAlwaysActive";
        private const string HUD_HIDDEN_KEY = "HUDHidden";
        private const string HUD_ALPHA_KEY = "HUDAlpha";

        private AudioManager m_audioManager;

        private float openTime;

        private void Start()
        {
            m_audioManager = FindObjectOfType<AudioManager>();
            if (m_audioManager == null)
            {
                Debug.LogError("AudioManager no encontrado. Asegúrate de que esté presente en la escena.");
            }

            ApplyHUDSettings();
        }

        public void Initialize()
        {
            controlsScreen.Close();

            if (masterSlider != null)
                masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            else
                Debug.LogError("Master Slider no asignado en el inspector.");

            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            else
                Debug.LogError("Music Slider no asignado en el inspector.");

            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            else
                Debug.LogError("SFX Slider no asignado en el inspector.");

            if (hudAlwaysActiveToggle != null)
                hudAlwaysActiveToggle.onValueChanged.AddListener(ToggleOnAlwaysActive);
            else
                Debug.LogError("HUD Always Active Toggle no asignado en el inspector.");

            if (hudHiddenToggle != null)
                hudHiddenToggle.onValueChanged.AddListener(ToggleHudHidden);
            else
                Debug.LogError("HUD Hidden Toggle no asignado en el inspector.");

            if (alphaHUDSlider != null)
                alphaHUDSlider.onValueChanged.AddListener(AlphaHUDValueChanged);
            else
                Debug.LogError("Alpha HUD Slider no asignado en el inspector.");

            if (controlsScreenButton != null)
                controlsScreenButton.onClick.AddListener(OpenControlPanel);
            else
                Debug.LogError("Control Screen Button no asignado en el inspector.");

            if (goBackControlButton != null)
                goBackControlButton.onClick.AddListener(CloseControlPanel);
            else
                Debug.LogError("Go Back Control Button no asignado en el inspector.");

            if (goBackScreenButton != null)
                goBackScreenButton.onClick.AddListener(Close);
            else
                Debug.LogError("Go Back Screen Button no asignado en el inspector.");
        }

        private void OpenControlPanel()
        {
            controlsScreen.Open();
        }

        private void CloseControlPanel()
        {
            controlsScreen.Close();
        }

        public override void Open()
        {
            openTime = Time.time;

            if (masterSlider != null)
                masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));

            if (musicSlider != null)
                musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1));

            if (sfxSlider != null)
                sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1));

            base.Open();
        }

        public override void Close()
        {
            float duration = Time.time - openTime;

            if (UnityServices.State == ServicesInitializationState.Initialized && AnalyticsService.Instance != null)
            {
                AnalyticsService.Instance.CustomData("Menu_Screen_Time", new Dictionary<string, object>
                {
                    { "ScreenType", "Settings" },
                    { "Duration", duration }
                });

                AnalyticsService.Instance.Flush();
                Debug.Log($"Evento 'Menu_Screen_Time' enviado para Settings con duración {duration} segundos.");
            }
            else
            {
                Debug.LogWarning("Unity Services no está inicializado. No se enviará el evento de Analytics.");
            }

            base.Close();
        }

        private void ApplyHUDSettings()
        {
            if (hudHiddenToggle != null)
            {
                hudHiddenToggle.isOn = PlayerPrefs.GetInt(HUD_HIDDEN_KEY, 1) == 1;
            }

            if (hudAlwaysActiveToggle != null)
            {
                hudAlwaysActiveToggle.isOn = PlayerPrefs.GetInt(HUD_ALWAYS_ACTIVE_KEY, 0) == 1;
            }

            if (alphaHUDSlider != null)
            {
                alphaHUDSlider.value = PlayerPrefs.GetFloat(HUD_ALPHA_KEY, 1);
            }

            if (hudCanvasGroup != null)
            {
                hudCanvasGroup.alpha = alphaHUDSlider != null ? alphaHUDSlider.value : 1;
                hudCanvasGroup.interactable = hudAlwaysActiveToggle != null && hudAlwaysActiveToggle.isOn;
                hudCanvasGroup.blocksRaycasts = hudHiddenToggle == null || !hudHiddenToggle.isOn;
            }
        }

        private void OnMasterVolumeChanged(float value)
        {
            if (m_audioManager == null)
            {
                Debug.LogError("AudioManager no está inicializado. Asegúrate de que esté disponible.");
                return;
            }

            PlayerPrefs.SetFloat("MasterVolume", value);
            m_audioManager.SetMasterVolume(value);
            m_audioManager.mixer.SetFloat("MasterVolume", m_audioManager.LinearToDecibel(value));
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (m_audioManager == null)
            {
                Debug.LogError("AudioManager no está inicializado. Asegúrate de que esté disponible.");
                return;
            }

            PlayerPrefs.SetFloat("MusicVolume", value);
            m_audioManager.SetMusicVolume(value);
            m_audioManager.mixer.SetFloat("MusicVolume", m_audioManager.LinearToDecibel(value));
        }

        private void OnSFXVolumeChanged(float value)
        {
            if (m_audioManager == null)
            {
                Debug.LogError("AudioManager no está inicializado. Asegúrate de que esté disponible.");
                return;
            }

            PlayerPrefs.SetFloat("SFXVolume", value);
            m_audioManager.SetSfxVolume(value);
            m_audioManager.mixer.SetFloat("SFXVolume", m_audioManager.LinearToDecibel(value));
        }

        private void ToggleOnAlwaysActive(bool value)
        {
            PlayerPrefs.SetInt(HUD_ALWAYS_ACTIVE_KEY, value ? 1 : 0);
            ApplyHUDSettings();
        }

        private void ToggleHudHidden(bool value)
        {
            PlayerPrefs.SetInt(HUD_HIDDEN_KEY, value ? 1 : 0);
            ApplyHUDSettings();
        }

        private void AlphaHUDValueChanged(float value)
        {
            PlayerPrefs.SetFloat(HUD_ALPHA_KEY, value);
            ApplyHUDSettings();
        }
    }
}

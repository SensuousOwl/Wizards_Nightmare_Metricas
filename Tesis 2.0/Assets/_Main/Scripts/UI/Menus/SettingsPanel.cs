using _Main.Scripts.Audio;
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

        private InputAction m_toggleHHudInputAction;

        private void Start()
        {
            var l_inputManager = InputManager.Instance;
            if(l_inputManager!=default)
                m_toggleHHudInputAction = l_inputManager.GetInputAction("ToggleHUD");
            
            ApplyHUDSettings();
        }

        public void Initialize()
        {

            controlsScreen.Close();

            m_audioManager = FindObjectOfType<AudioManager>();
            
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        
            hudAlwaysActiveToggle.onValueChanged.AddListener(ToggleOnAlwaysActive);
            hudHiddenToggle.onValueChanged.AddListener(ToggleHudHidden);
            alphaHUDSlider.onValueChanged.AddListener(AlphaHUDValueChanged);
        
            controlsScreenButton.onClick.AddListener(OpenControlPanel);
            goBackControlButton.onClick.AddListener(CloseControlPanel);
            goBackScreenButton.onClick.AddListener(() => Close());
   
        
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
            base.Open();
        
            //TODO: levantar de los datos si esta prendido o apagdo
            // hudHiddenToggle.isOn = PlayerPrefs.GetInt(HudHiddenKey, defaultValue: 0) == 1;
            // hudAlwaysActiveToggle.isOn = PlayerPrefs.GetInt(HudAlwaysActiveKey, defaultValue: 1) == 1;
            // alphaHUDSlider.value = PlayerPrefs.GetFloat(HudAlphaKey, defaultValue: 1);
            //
            // ApplyHUDSettings();
            
            masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));
            musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1));
            sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVolume", 1));
        }
        
        private void ApplyHUDSettings()
        {
            hudHiddenToggle.isOn = PlayerPrefs.GetInt(HUD_HIDDEN_KEY, defaultValue: 0) == 1;
            hudAlwaysActiveToggle.isOn = PlayerPrefs.GetInt(HUD_ALWAYS_ACTIVE_KEY, defaultValue: 1) == 1;
            if (hudCanvasGroup != default)
            {
                hudCanvasGroup.alpha = alphaHUDSlider.value;
                hudCanvasGroup.interactable = hudAlwaysActiveToggle.isOn;
                hudCanvasGroup.blocksRaycasts = !hudHiddenToggle.isOn;
                hudCanvasGroup.gameObject.SetActive(hudHiddenToggle.isOn);
                hudCanvasGroup.gameObject.SetActive(hudAlwaysActiveToggle.isOn);
            }
            
            alphaHUDSlider.value = PlayerPrefs.GetFloat(HUD_ALPHA_KEY, defaultValue: 1);
        }

        private void OnMasterVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            m_audioManager.SetMasterVolume(value);
            m_audioManager.mixer.SetFloat("MasterVolume", m_audioManager.LinearToDecibel(value));
        }
        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            m_audioManager.SetMusicVolume(value);
            m_audioManager.mixer.SetFloat("MusicVolume", m_audioManager.LinearToDecibel(value));
        }
        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            m_audioManager.SetSFXVolume(value);
            m_audioManager.mixer.SetFloat("SFXVolume", m_audioManager.LinearToDecibel(value));
        }

        private void ToggleOnAlwaysActive(bool p_value)
        {
            PlayerPrefs.SetInt(HUD_ALWAYS_ACTIVE_KEY, p_value ? 1 : 0);
            ApplyHUDSettings();
            
            UnSubscribeToggleHudInput();
            if(hudCanvasGroup == default)
                return;
            hudCanvasGroup.gameObject.SetActive(true);
            
            
        }
    
        private void ToggleHudHidden(bool p_value)
        {
            PlayerPrefs.SetInt(HUD_HIDDEN_KEY, p_value ? 1 : 0);
            ApplyHUDSettings();

            if (p_value)
            {
                SubscribeToggleHudInput();
                if(hudCanvasGroup == default)
                    return;
                hudCanvasGroup.gameObject.SetActive(false);
            }
            else
                UnSubscribeToggleHudInput();
        }

        private void SubscribeToggleHudInput()
        {
            if(m_toggleHHudInputAction==default)
                return;
            
            m_toggleHHudInputAction.started += ToggleHud;
            m_toggleHHudInputAction.canceled += ToggleHud;
        }
        
        private void UnSubscribeToggleHudInput()
        {
            if(m_toggleHHudInputAction==default)
                return;
            m_toggleHHudInputAction.started -= ToggleHud;
            m_toggleHHudInputAction.canceled -= ToggleHud;
        }
        private void ToggleHud(InputAction.CallbackContext p_obj)
        {
            hudCanvasGroup.gameObject.SetActive(!hudCanvasGroup.isActiveAndEnabled);
        }

        private void AlphaHUDValueChanged(float value)
        {
            PlayerPrefs.SetFloat(HUD_ALPHA_KEY, value);
            ApplyHUDSettings();
        }
    }
}

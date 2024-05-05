using _Main.Scripts.Audio;
using UnityEngine;
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

        private const string HudAlwaysActiveKey = "HUDAlwaysActive";
        private const string HudHiddenKey = "HUDHidden";
        private const string HudAlphaKey = "HUDAlpha";

        private AudioManager audioManager;

        private void Start()
        {
            hudHiddenToggle.isOn = PlayerPrefs.GetInt(HudHiddenKey, defaultValue: 0) == 1;
            hudAlwaysActiveToggle.isOn = PlayerPrefs.GetInt(HudAlwaysActiveKey, defaultValue: 1) == 1;
            alphaHUDSlider.value = PlayerPrefs.GetFloat(HudAlphaKey, defaultValue: 1);

            ApplyHUDSettings();
        }

        public void Initialize()
        {

            controlsScreen.Close();

            audioManager = FindObjectOfType<AudioManager>();
            
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
            if (hudCanvasGroup != null)
            {
                hudCanvasGroup.alpha = alphaHUDSlider.value;
                hudCanvasGroup.interactable = hudAlwaysActiveToggle.isOn;
                hudCanvasGroup.blocksRaycasts = !hudHiddenToggle.isOn;
                
                // Additional logic to hide/show HUD based on hudHiddenToggle if necessary
            }
        }

        private void OnMasterVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            audioManager.SetMasterVolume(value);
            audioManager.mixer.SetFloat("MasterVolume", audioManager.LinearToDecibel(value));
        }
        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            audioManager.SetMusicVolume(value);
            audioManager.mixer.SetFloat("MusicVolume", audioManager.LinearToDecibel(value));
        }
        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            audioManager.SetSFXVolume(value);
            audioManager.mixer.SetFloat("SFXVolume", audioManager.LinearToDecibel(value));
        }

        private void ToggleOnAlwaysActive(bool value)
        {
            PlayerPrefs.SetInt(HudAlwaysActiveKey, value ? 1 : 0);
            ApplyHUDSettings();
        }
    
        private void ToggleHudHidden(bool value)
        {
            PlayerPrefs.SetInt(HudHiddenKey, value ? 1 : 0);
            ApplyHUDSettings();
        }
    
        private void AlphaHUDValueChanged(float value)
        {
            PlayerPrefs.SetFloat(HudAlphaKey, value);
            ApplyHUDSettings();
        }
    }
}

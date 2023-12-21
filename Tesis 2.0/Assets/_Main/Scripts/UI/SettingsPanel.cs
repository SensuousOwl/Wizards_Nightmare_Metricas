using _Main.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.UI
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

        private AudioManager audioManager;

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
            hudHiddenToggle.isOn = false;
            hudAlwaysActiveToggle.isOn = false;
        
            //TODO: levantar valores del AudioManager
            masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));
            musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));
            sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));
        }

        private void OnMasterVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            audioManager.SetMasterVolume(value);
            audioManager.mixer.SetFloat("MasterVolume", audioManager.LinearToDecibel(value));
            print($"Master Volume: {value}");
        }
        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            audioManager.SetMusicVolume(value);
            audioManager.mixer.SetFloat("MusicVolume", audioManager.LinearToDecibel(value));
            print($"Music Volume: {value}");
        }
        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            audioManager.SetSFXVolume(value);
            audioManager.mixer.SetFloat("SFXVolume", audioManager.LinearToDecibel(value));
            print($"SFX Volume: {value}");
        }

        private void ToggleOnAlwaysActive(bool value)
        {
            print($"Toggle Active: {value}");
        }
    
        private void ToggleHudHidden(bool value)
        {
            print($"Toggle Hidden: {value}");
        }
    
        private void AlphaHUDValueChanged(float value)
        {
            print($"Alpha HUD: {value}");
        }
    }
}

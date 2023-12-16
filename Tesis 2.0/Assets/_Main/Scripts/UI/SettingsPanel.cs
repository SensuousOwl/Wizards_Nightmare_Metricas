using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void Initialize()
    {
        controlsScreen.Close();
        
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
        masterSlider.SetValueWithoutNotify(1);
        musicSlider.SetValueWithoutNotify(1);
        sfxSlider.SetValueWithoutNotify(1);
    }

    private void OnMasterVolumeChanged(float value)
    {
        print($"Master Volume: {value}");
    }
    private void OnSFXVolumeChanged(float value)
    {
        print($"SFX Volume: {value}");
    }
    private void OnMusicVolumeChanged(float value)
    {
        print($"Music Volume: {value}");
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

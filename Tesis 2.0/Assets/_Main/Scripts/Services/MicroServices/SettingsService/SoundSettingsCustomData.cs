using System;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.SettingsService
{
    [Serializable]
    public class SoundSettingsCustomData : ISettingCustomData
    {
        [JsonProperty] public float MasterVolume { get; set; }
        [JsonProperty] public float MusicVolume { get; set; }
        [JsonProperty] public float EffectsVolume { get; set; }
    }
}
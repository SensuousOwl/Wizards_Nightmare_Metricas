using System;
using System.Collections.Generic;
using _Main.Scripts.Services.MicroServices.UserDataService;
using Unity.Plastic.Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.SettingsService
{
    [Serializable]
    public class SettingsState : IUserState
    {
        [JsonProperty] private Dictionary<Type, ISettingCustomData> m_data = new();

        public bool TryGetSettingData<T>(out T p_settingData) where T : ISettingCustomData
        {
            p_settingData = default;
            var l_typeId = typeof(T);
            if (!m_data.TryGetValue(l_typeId, out var l_settingData))
                return false;

            p_settingData = (T)l_settingData;

            return true;
        }

        public void AddSettingData<T>(T p_settingData) where T: ISettingCustomData
        {
            var l_typeId = typeof(T);
            if (m_data.ContainsKey(l_typeId))
                return;

            m_data[l_typeId] = p_settingData;
        }
        
        public void RemoveSettingData(Type p_settingId)
        {
            if (!m_data.ContainsKey(p_settingId))
                return;

            m_data.Remove(p_settingId);
        }
    }
}
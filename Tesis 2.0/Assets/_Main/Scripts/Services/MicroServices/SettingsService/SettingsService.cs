using System;
using _Main.Scripts.Services.MicroServices.UserDataService;

namespace _Main.Scripts.Services.MicroServices.SettingsService
{
    public class SettingsService : ISettingsService
    {
        private static IUserDataService UserDataService => ServiceLocator.Get<IUserDataService>();
        private static SettingsState SettingState => UserDataService.GetState<SettingsState>();
        
        public void Initialize()
        {
        }

        public T GetSettingData<T>() where T : ISettingCustomData, new()
        {
            if (SettingState.TryGetSettingData(out T l_settingData))
                return l_settingData;

            var l_newSettingData = new T();
            SettingState.AddSettingData(l_newSettingData);
            return l_newSettingData;
        }

        public void RemoveSettingData(Type p_settingId)
        {
            SettingState.RemoveSettingData(p_settingId);
        }
    }
}
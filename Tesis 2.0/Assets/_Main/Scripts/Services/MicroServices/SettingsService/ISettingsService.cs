using System;

namespace _Main.Scripts.Services.MicroServices.SettingsService
{
    public interface ISettingsService : IGameService
    {
        public T GetSettingData<T>() where T : ISettingCustomData, new();
        public void RemoveSettingData(Type p_settingId);
    }
}
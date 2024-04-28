using System;
using Unity.VisualScripting;

namespace _Main.Scripts.Services.CurrencyServices
{
    public interface ICurrencyService : IGameService
    {
        public Action<int> OnCurrencyChange  { get; set; }
        int GetCurrentGs();
        void SetGs(int p_newGs);
        void AddGs(int p_GsToAdd);
    }
}
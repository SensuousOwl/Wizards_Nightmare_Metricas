using System;
using _Main.Scripts.Services.MicroServices.UserDataService;

namespace _Main.Scripts.Services.CurrencyServices
{
    public class CurrencyService : ICurrencyService
    {
        
        private CurrencyDataService DataState =>
            ServiceLocator.Get<IUserDataService>().GetState<CurrencyDataService>();

        
        public void Initialize()
        {
            if(DataState.GetCurrentGs() != default)
                return;
            
            DataState.ResetGs();
        }

        public Action<int> OnCurrencyChange { get; set; }
        public int GetCurrentGs() => DataState.GetCurrentGs();

        public void SetGs(int p_newGs)
        {

            if (p_newGs >= 0)
            {
                DataState.SetGs(p_newGs);
                OnCurrencyChange.Invoke(p_newGs);
                return;
            }

            Logger.LogError("Modification on Gs returned a value under 0");
            
        }

        public void AddGs(int p_GsToAdd)
        {
            var newAmmount = DataState.GetCurrentGs() + p_GsToAdd;

            if (newAmmount >= 0)
            {
                DataState.SetGs(newAmmount);
                OnCurrencyChange.Invoke(newAmmount);
                return;
            }
            Logger.LogError("Modification on Gs returned a value under 0");
        }

        
    }
}
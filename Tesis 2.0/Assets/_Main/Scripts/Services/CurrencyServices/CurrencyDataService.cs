using System.Collections.Generic;
using System.Linq.Expressions;
using _Main.Scripts.Services.MicroServices.UserDataService;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.CurrencyServices
{
    public class CurrencyDataService : IUserState
    {
        [JsonProperty] private int currentGs;


        public void ResetGs() => currentGs = 0;
        public int GetCurrentGs() => currentGs;
        public void SetGs(int p_newGs) => currentGs = p_newGs;
    }
}
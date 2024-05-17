using _Main.Scripts.Services.MicroServices.UserDataService;
using Newtonsoft.Json;

namespace _Main.Scripts.Services.CurrencyServices
{
    public class CurrencyDataService : IUserState
    {
        [JsonProperty] private int m_currentGs;


        public void ResetGs() => m_currentGs = 0;
        public int GetCurrentGs() => m_currentGs;
        public void SetGs(int p_newGs) => m_currentGs = p_newGs;
    }
}
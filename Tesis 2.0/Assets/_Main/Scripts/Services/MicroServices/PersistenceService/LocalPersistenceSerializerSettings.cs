using Unity.Plastic.Newtonsoft.Json;

namespace _Main.Scripts.Services.MicroServices.PersistenceService
{
    public class LocalPersistenceSerializerSettings : JsonSerializerSettings
    {
        public LocalPersistenceSerializerSettings()
        {
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.Auto;
            NullValueHandling = NullValueHandling.Include;
            DefaultValueHandling = DefaultValueHandling.Include;
            ContractResolver = new LocalPersistenceContractResolver();
            PreserveReferencesHandling = PreserveReferencesHandling.None;
        }
    }
}
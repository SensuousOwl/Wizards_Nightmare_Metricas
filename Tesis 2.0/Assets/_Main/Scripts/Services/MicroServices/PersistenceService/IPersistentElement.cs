namespace _Main.Scripts.Services.MicroServices.PersistenceService
{
    public interface IPersistentElement
    {
        string PersistenceID { get; }
        void OnAfterDeserialize();
        void OnBeforeSerialize();
    }
}
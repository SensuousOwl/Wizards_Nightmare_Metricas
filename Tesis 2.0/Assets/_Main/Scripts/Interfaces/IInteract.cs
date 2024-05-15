using _Main.Scripts.PlayerScripts;

namespace _Main.Scripts.Interfaces
{
    public interface IInteract
    {
        public void Interact(PlayerModel p_model);
        public void ShowCanvasUI(bool p_b);
    }
}
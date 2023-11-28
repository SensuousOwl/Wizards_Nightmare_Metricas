using _Main.Scripts.Managers;

namespace _Main.Scripts.Interfaces
{
    public interface IPausable
    {
        public void SubscribePause();

        public void UnsubscribePause();

        public void Pause(bool p_pauseState);
    }
}
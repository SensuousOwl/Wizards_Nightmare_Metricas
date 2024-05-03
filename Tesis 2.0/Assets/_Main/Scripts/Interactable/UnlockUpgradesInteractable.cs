using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Interactable
{
    public class UnlockUpgradesInteractable : MonoBehaviour, IInteract
    {
        [SerializeField] private int gemsToUnlock; 
        
        public void Interact(PlayerModel p_model)
        {
        }
    }
}
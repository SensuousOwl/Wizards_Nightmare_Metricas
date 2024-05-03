using _Main.Scripts.Interfaces;
using _Main.Scripts.PlayerScripts;
using UnityEngine;

namespace _Main.Scripts.Interactable
{
    public class ShowCreditsUiInteractable : MonoBehaviour, IInteract
    {
        [SerializeField] private GameObject uiGameObject;
        public void Interact(PlayerModel p_model)
        {
            uiGameObject.SetActive(true);
            //desabilitar el control del
        }
    }
}
using System;
using _Main.Scripts.UI;
using UnityEngine;

namespace _Main.Scripts
{
    public class XpController : MonoBehaviour
    {
        [SerializeField] private UpgradeScreenController ScreenController;


        public static Action OnLvlUp;



        private void OnEnable()
        {
            OnLvlUp += OnPlayerLvlUp;
        }

        private void OnDisable()
        {
            OnLvlUp -= OnPlayerLvlUp;
        }

        private void OnPlayerLvlUp()
        {
            ScreenController.ActivateUpgradeScreen();
        }
    }
}
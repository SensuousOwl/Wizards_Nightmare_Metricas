using System;
using _Main._main.Scripts.Extensions;
using temp;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputData inputData;

        private PlayerModel m_model;
        private Vector2 m_currDir;
        private void Start()
        {
            var lInputManager = InputManager.Instance;
            
            lInputManager.SubscribeInput(inputData.AimId, OnAimPerformed);
            lInputManager.SubscribeInput(inputData.DashId, OnDashPerformed);
            lInputManager.SubscribeInput(inputData.MovementId, OnMovementPerformed);
            lInputManager.SubscribeInput(inputData.ShootId, OnShootPerformed);

            m_model = GetComponent<PlayerModel>();
        }


        private void Update()
        {
            m_model.Move(m_currDir);
        }

        private void OnShootPerformed(InputAction.CallbackContext p_obj)
        {
            m_model.Shoot();
        }

        private void OnMovementPerformed(InputAction.CallbackContext p_obj)
        {
            m_currDir = p_obj.ReadValue<Vector2>();
            m_currDir.Normalize();
        }

        private void OnDashPerformed(InputAction.CallbackContext p_obj)
        {
            m_model.Dash(m_currDir.normalized);
        }

        private void OnAimPerformed(InputAction.CallbackContext p_obj)
        {
            var pos = p_obj.ReadValue<Vector2>();
            m_model.UpdateCrossAir(pos);
        }
    }
}
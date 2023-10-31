using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputData inputData;

        private PlayerModel m_model;
        private bool m_isShooting;
        private Vector2 m_currDir;
        private void Start()
        {
            SubscribeInputs();
            
            m_model = GetComponent<PlayerModel>();
        }

        private void SubscribeInputs()
        {
            
            var lInputManager = InputManager.Instance;
            
            lInputManager.SubscribeInput(inputData.AimId, OnAimPerformed);
            lInputManager.SubscribeInput(inputData.DashId, OnDashPerformed);
            lInputManager.SubscribeInput(inputData.MovementId, OnMovementPerformed);
            lInputManager.SubscribeInput(inputData.ShootId, OnShootPerformed);

            lInputManager.GetInputAction(inputData.MovementId).canceled += OnMovementPerformed;
            lInputManager.GetInputAction(inputData.ShootId).canceled += OnShootCanceled;
        }

        private void Update()
        {
            m_model.Move(m_currDir);

            if (m_isShooting)
            {
                m_model.Shoot();
            }
        }

        private void OnShootPerformed(InputAction.CallbackContext p_obj)
        {
            m_isShooting = true;
        }
        
        private void OnShootCanceled(InputAction.CallbackContext p_obj)
        {
            m_isShooting = false;
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
using System;
using _Main.Scripts.Entities.PlayerScripts.Data;
using _Main.Scripts.Interfaces;
using _Main.Scripts.Managers;
using _Main.Scripts.Services;
using _Main.Scripts.Services.MicroServices.InventoryService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Entities.PlayerScripts.MVC
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerController : MonoBehaviour, IPlayerController, IPausable
    {
        [SerializeField] private PlayerInputData inputData;

        private bool m_isShooting;
        private Vector2 m_currDir;

        public event Action OnUseItem;
        public event Action OnInteract;
        public event Action OnShoot;
        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnDash;
        public event Action<Vector2> OnUpdateCrosshair;

        private bool m_isPause;

        private static IInventoryService InventoryService => ServiceLocator.Get<IInventoryService>();
        
        private void Start()
        {
            SubscribeInputs();
            
            SubscribePause();
        }

        private void OnDisable()
        {
            UnsubscribeInputs();
        }
        
        private void Update()
        {
            if (m_isPause)
                return;
            
            if (m_isShooting)
            {
                OnShoot?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (m_isPause)
                return;
            
            OnMove?.Invoke(m_currDir);
        }

        private void SubscribeInputs()
        {
            var l_lInputManager = InputManager.Instance;
            
            l_lInputManager.SubscribeInput(inputData.UseItemId, OnUseItemPerformed);
            l_lInputManager.SubscribeInput(inputData.InteractId, OnInteractPerformed);
            l_lInputManager.SubscribeInput(inputData.AimId, OnAimPerformed);
            l_lInputManager.SubscribeInput(inputData.DashId, OnDashPerformed);
            l_lInputManager.SubscribeInput(inputData.MovementId, OnMovementPerformed);
            l_lInputManager.SubscribeInput(inputData.ShootId, OnShootPerformed);
            l_lInputManager.SubscribeInput(inputData.DropItemActive, OnDropActiveItemPerformed);
            l_lInputManager.SubscribeInput(inputData.DropItemPassive, OnDropPassiveItemPerformed);

            l_lInputManager.GetInputAction(inputData.MovementId).canceled += OnMovementPerformed;
            l_lInputManager.GetInputAction(inputData.ShootId).canceled += OnShootCanceled;
        }

        private void UnsubscribeInputs()
        {
            var l_lInputManager = InputManager.Instance;
            
            l_lInputManager.UnsubscribeInput(inputData.UseItemId, OnUseItemPerformed);
            l_lInputManager.UnsubscribeInput(inputData.AimId, OnInteractPerformed);
            l_lInputManager.UnsubscribeInput(inputData.AimId, OnAimPerformed);
            l_lInputManager.UnsubscribeInput(inputData.DashId, OnDashPerformed);
            l_lInputManager.UnsubscribeInput(inputData.MovementId, OnMovementPerformed);
            l_lInputManager.UnsubscribeInput(inputData.ShootId, OnShootPerformed);
            l_lInputManager.UnsubscribeInput(inputData.DropItemActive, OnDropActiveItemPerformed);
            l_lInputManager.UnsubscribeInput(inputData.DropItemPassive, OnDropPassiveItemPerformed);

            l_lInputManager.GetInputAction(inputData.MovementId).canceled -= OnMovementPerformed;
            l_lInputManager.GetInputAction(inputData.ShootId).canceled -= OnShootCanceled;
        }

        private void OnDropActiveItemPerformed(InputAction.CallbackContext p_obj)
        {
            InventoryService.DropActiveItem(transform.position);
        }
        private void OnDropPassiveItemPerformed(InputAction.CallbackContext p_obj)
        {
            InventoryService.DropPassiveItem(transform.position);
        }
        
        private void OnUseItemPerformed(InputAction.CallbackContext p_obj)
        {
            if (m_isPause)
                return;
            
            OnUseItem?.Invoke();
        }

        private void OnInteractPerformed(InputAction.CallbackContext p_obj)
        {
            if (m_isPause)
                return;
            
            OnInteract?.Invoke();
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
            if (m_isPause)
                return;
            
            m_currDir = p_obj.ReadValue<Vector2>();
            m_currDir.Normalize();
        }

        private void OnDashPerformed(InputAction.CallbackContext p_obj)
        {
            if (m_isPause)
                return;
            
            OnDash?.Invoke(m_currDir.normalized);
        }

        private void OnAimPerformed(InputAction.CallbackContext p_obj)
        {
            if (m_isPause)
                return;
            
            var l_pos = p_obj.ReadValue<Vector2>();
            OnUpdateCrosshair?.Invoke(l_pos);
        }

        public void SubscribePause()
        {
            //PauseManager.Instance.Subscribe(this);
        }

        public void UnsubscribePause()
        {
            //PauseManager.Instance.Unsubscribe(this);
        }

        public void Pause(bool p_pauseState)
        {
            m_isPause = p_pauseState;
        }
    }
}
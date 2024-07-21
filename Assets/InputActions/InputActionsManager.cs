using System;
using PlayerSpace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputActionsSpace
{
    public class InputActionsManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        private InputAction _actionRun;
        private InputAction _actionJump;
        private InputAction _actionWalk;
        private InputAction _actionEscape;
        private InputAction _actionUseSmth;
        private InputAction _actionShowInventory;
        
        public static event Action OnPressedEscapeEvent;
        public static event Action OnPressedUseSmthEvent;
        public static event Action OnPressedShowInventoryEvent;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            _actionRun = _playerInput.actions["Run"];
            _actionJump = _playerInput.actions["Jump"];
            _actionWalk = _playerInput.actions["Walk"];
            _actionEscape = _playerInput.actions["Escape"];
            _actionUseSmth = _playerInput.actions["UseSmth"];
            _actionShowInventory = _playerInput.actions["ShowInventory"];
        }
        
        private void OnEnable()
        {
            _actionRun.performed += Run;
            _actionRun.canceled += Run;
            
            _actionJump.performed += Jump;
            _actionEscape.performed += Escape;
            _actionUseSmth.performed += UseSmth;
            _actionShowInventory.performed += ShowInventory;
        }

        private void Update()
        {
            PlayerMovement.Instance.IsWalking = _actionWalk.IsPressed();
        }
        
        private void ShowInventory(InputAction.CallbackContext obj)
        {
            OnPressedShowInventoryEvent?.Invoke();
        }

        private void UseSmth(InputAction.CallbackContext obj)
        {
            OnPressedUseSmthEvent?.Invoke();
        }

        private void Escape(InputAction.CallbackContext obj)
        {
            OnPressedEscapeEvent?.Invoke();
        }
        
        private void Run(InputAction.CallbackContext obj)
        {
            PlayerMovement.Instance.MoveInput = obj.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            PlayerMovement.Instance.Jump();
        }

        private void OnDisable()
        {
            _actionRun.performed -= Run;
            _actionRun.canceled -= Run;
            
            _actionJump.performed -= Jump;
            _actionEscape.performed -= Escape;
            _actionUseSmth.performed -= UseSmth;
            _actionShowInventory.performed -= ShowInventory;
        }
    }
}

using Cinemachine;
using InputActionsSpace;
using UnityEngine;

namespace GameControllerSpace
{
    public enum PossibleActionEnum
    {
        None
    }
    
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        
        [SerializeField] private GameObject _inventoryObj;
        
        private bool IsOpenedMenu { get; set; }
        private bool IsOpenedInventory { get; set; }
        private PossibleActionEnum PossibleAction { get; set; }
        private CinemachineBrain _cameraBrain;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            InputActionsManager.OnPressedEscapeEvent += OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent += OnPressedUseSmth;
            InputActionsManager.OnPressedShowInventoryEvent += OnPressedShowInventory;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _cameraBrain = Camera.main?.GetComponent<CinemachineBrain>();
        }
        
        private void OnPressedShowInventory()
        {
            IsOpenedInventory = !IsOpenedInventory;

            LockCamera(!IsOpenedInventory);
            
            Cursor.lockState = IsOpenedInventory ? CursorLockMode.Confined : CursorLockMode.Locked; 
            
            _inventoryObj.SetActive(IsOpenedInventory);
        }

        private void LockCamera(bool isLock)
        {
            _cameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineInputProvider>().enabled = isLock;
        }
        
        private void OnPressedUseSmth()
        {
            switch (PossibleAction)
            {
                case PossibleActionEnum.None:
                default:
                    return;
            }
        }
        
        private void OnToggleMenu()
        {
            IsOpenedMenu = !IsOpenedMenu;
        }
        
        private void OnDestroy()
        {
            InputActionsManager.OnPressedEscapeEvent -= OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent -= OnPressedUseSmth;
            InputActionsManager.OnPressedShowInventoryEvent -= OnPressedShowInventory;
        }
    }
}


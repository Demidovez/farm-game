using InventoryScriptableObjectSpace;
using InventorySpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        [SerializeField] private InventorySO _inventorySo;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InventoryItemHolder inventoryItemHolder))
            {
                var inventoryItem = inventoryItemHolder.InventoryItemSO.Create();
                
                _inventorySo.AddItem(inventoryItem, 1);
                
                Destroy(other.gameObject);
            }
        }
    }
}


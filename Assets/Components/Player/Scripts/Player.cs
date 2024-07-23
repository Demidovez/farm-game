using InventoryScriptableObjectSpace;
using InventorySpace;
using PlantSpace;
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
            if (other.TryGetComponent(out Plant plant))
            {
                if (plant.IsReady)
                {
                    var inventoryItem = plant.CreateInventoryItem();
                
                    _inventorySo.AddItem(inventoryItem, 1);
                }
            }
        }
    }
}


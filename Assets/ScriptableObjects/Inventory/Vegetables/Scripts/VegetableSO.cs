using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Vegetable", menuName = "Inventory System/Inventory Items/Vegetable")]
    public class VegetableSO : InventoryItemSO
    {
        public GameObject MaturationStep1;
        public GameObject MaturationStep2;
        public GameObject MaturationStep3;
        public GameObject MaturationStep4;
        
        public VegetableSO()
        {
            Type = EInventoryItem.Vegetable;
        }
    } 
}


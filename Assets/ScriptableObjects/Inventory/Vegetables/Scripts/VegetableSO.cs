using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Vegetable", menuName = "Inventory System/Inventory Items/Vegetable")]
    public class VegetableSO : InventoryItemSO
    {
        public VegetableSO()
        {
            Type = EInventoryItem.Vegetable;
        }
    } 
}


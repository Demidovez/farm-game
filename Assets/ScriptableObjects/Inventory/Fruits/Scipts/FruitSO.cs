using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Fruit", menuName = "Inventory System/Inventory Items/Fruit")]
    public class FruitSO : InventoryItemSO
    {
        public FruitSO()
        {
            Type = EInventoryItem.Fruit;
        }
    } 
}


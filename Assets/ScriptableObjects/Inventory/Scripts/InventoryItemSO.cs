using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    public enum EInventoryItem
    {
        Fruit,
        Vegetable,
        RootCrop, 
        Cereals,
        Tool
    }
    
    public abstract class InventoryItemSO : ScriptableObject
    {
        public int Id;
        public string Name;
        public Sprite UiDisplay;
        public EInventoryItem Type;

        [TextArea(15, 20)] public string Description;

        public InventoryItem Create()
        {
            InventoryItem newItem = new InventoryItem(this);

            return newItem;
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public int Id;
        public string Name;

        public InventoryItem(InventoryItemSO itemSO)
        {
            Id = itemSO.Id;
            Name = itemSO.Name;
        }
        
        public InventoryItem()
        {
            Id = 0;
            Name = "";
        }

        public static InventoryItem GetEmpty()
        {
            return new InventoryItem();
        }
    }
}

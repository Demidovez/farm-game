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
        public string Id = System.Guid.NewGuid().ToString();
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
        public string Id;
        public string Name;

        public InventoryItem(InventoryItemSO itemSO)
        {
            Id = itemSO.Id;
            Name = itemSO.Name;
        }
    }
}

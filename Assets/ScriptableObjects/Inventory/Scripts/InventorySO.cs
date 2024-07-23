using System.Linq;
using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory System/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public Inventory Container;
        public InventoryDatabase Database;

        public Sprite GetSpriteByItemId(int id)
        {
            if (Database.Records.TryGetValue(id, out var slot))
            {
                return slot.UiDisplay;
            }
            
            return null;
        }

        public void AddItem(InventoryItem item, int amount)
        {
            foreach (var inventorySlot in Container.Items)
            {
                if (inventorySlot.Item.Id == item.Id)
                {
                    inventorySlot.AddAmount(amount);

                    return;
                }
            }
            
            SetEmptySlot(item, amount);
        }

        public void RemoveItem(InventoryItem item)
        {
            foreach (var inventorySlot in Container.Items)
            {
                if (inventorySlot.Item.Id == item.Id)
                {
                    inventorySlot.Clear();

                    return;
                }
            }
        }

        public void MoveItem(InventorySlot current, InventorySlot target)
        {
            InventorySlot temp = new InventorySlot(target);
            target.Update(current);
            current.Update(temp);
        }

        private void SetEmptySlot(InventoryItem item, int amount)
        {
            foreach (var inventorySlot in Container.Items)
            {
                if (inventorySlot.IsEmpty)
                {
                    inventorySlot.Update(item, amount);
                    
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class Inventory
    {
        public InventorySlot[] Items = new InventorySlot[28];
    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem Item;
        public int Amount;
        
        public bool IsEmpty => Item.Id == 0;

        public InventorySlot(InventoryItem item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        
        public InventorySlot(InventorySlot slot)
        {
            Item = slot.Item;
            Amount = slot.Amount;
        }

        public void Update(InventoryItem item, int amount = 0)
        {
            Item = item;
            Amount = amount;
        }
        
        public void Update(InventorySlot slot)
        {
            Item = slot.Item;
            Amount = slot.Amount;
        }

        public void Clear()
        {
            Item = InventoryItem.GetEmpty();
            Amount = 0;
        }

        public void AddAmount(int value)
        {
            Amount += value;
        }
    }
}

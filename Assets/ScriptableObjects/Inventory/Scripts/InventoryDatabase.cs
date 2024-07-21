using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Database", menuName = "Inventory System/Database")]
    public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public InventoryItemSO[] Items;
        internal List<InventoryItemSO> Records;
        
        public void OnBeforeSerialize()
        {
            Records = new List<InventoryItemSO>();
        }

        public void OnAfterDeserialize()
        {
            Records = new List<InventoryItemSO>();
            
            foreach (var inventoryItemSO in Items)
            {
                Records.Add(inventoryItemSO);
            }
        }
    }
}


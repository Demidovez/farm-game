using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Database", menuName = "Inventory System/Database")]
    public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public InventoryItemSO[] Items;
        internal Dictionary<string, InventoryItemSO> Records;
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Records = new();
            
            foreach (var inventoryItemSO in Items)
            {
                if (inventoryItemSO)
                {
                    Debug.Log(inventoryItemSO.Id);
                    Records.TryAdd(inventoryItemSO.Id, inventoryItemSO);
                }
            }
        }
    }
}


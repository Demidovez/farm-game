using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryScriptableObjectSpace
{
    [CreateAssetMenu(fileName = "Database", menuName = "Inventory System/Database")]
    public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public InventoryItemSO[] Items;
        internal Dictionary<int, InventoryItemSO> Records;
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Records = new();

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i])
                {
                    Records.TryAdd(i + 1, Items[i]);
                }
            }
        }
    }
}


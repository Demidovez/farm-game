using System.Collections.Generic;
using InventoryScriptableObjectSpace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace InventorySpace
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryCellPrefab;
        [SerializeField] private InventorySO _inventorySO;

        [SerializeField] private int _startX;
        [SerializeField] private int _startY;
        [SerializeField] private int _spaceGapHorizontal;
        [SerializeField] private int _spaceGapVertical;
        [SerializeField] private int _numberOfColumn;

        private Dictionary<GameObject, InventorySlot> _itemsDisplayed;
        private MouseItem _mouseItem;

        private void Start()
        {
            _mouseItem = new MouseItem();
        }

        private void OnEnable()
        {
            CreateSlots();
        }
        
        private void OnDisable()
        {
            DestroySlots();
        }

        private void CreateSlots()
        {
            _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0; i < _inventorySO.Container.Items.Length; i++)
            {
                var cellObj = Instantiate(_inventoryCellPrefab, Vector3.zero, transform.rotation, transform);
                
                cellObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                cellObj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
                
                AddEvent(cellObj, EventTriggerType.PointerEnter, delegate { OnEnter(cellObj); });
                AddEvent(cellObj, EventTriggerType.PointerExit, delegate { OnExit(cellObj); });
                AddEvent(cellObj, EventTriggerType.BeginDrag, delegate { OnDragBegin(cellObj); });
                AddEvent(cellObj, EventTriggerType.EndDrag, delegate { OnDragEnd(cellObj); });
                AddEvent(cellObj, EventTriggerType.Drag, delegate { OnDragged(cellObj); });
                
                _itemsDisplayed.Add(cellObj, _inventorySO.Container.Items[i]);
            }
        }
        
        private void DestroySlots()
        {
            foreach (var itemDisplayed in _itemsDisplayed)
            {
                Destroy(itemDisplayed.Key);
            }
            
            _itemsDisplayed = null;
        }

        private void OnEnter(GameObject cellObj)
        {
            _mouseItem.hoverObj = cellObj;

            if (_itemsDisplayed.TryGetValue(cellObj, out var slot))
            {
                _mouseItem.hoverSlot = slot;
            }
        }
        
        private void OnExit(GameObject cellObj)
        {
            Debug.Log("OnExit");
        }
        
        private void OnDragBegin(GameObject cellObj)
        {
            Debug.Log("OnDragBegin");
        }
        
        private void OnDragEnd(GameObject cellObj)
        {
            Debug.Log("OnDragEnd");
        }
        
        private void OnDragged(GameObject cellObj)
        {
            Debug.Log("OnDragged");
        }

        private Vector2 GetPosition(int index)
        {
            float x = _startX + _spaceGapHorizontal * (index % _numberOfColumn);
            float y = -(_startY + _spaceGapVertical * (index / _numberOfColumn));

            return new Vector2(x, y);
        }

        private void AddEvent(GameObject target, EventTriggerType triggerType, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>();
            
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = triggerType;
            eventTrigger.callback.AddListener(action);
            
            trigger.triggers.Add(eventTrigger);
        }
    }

    public class MouseItem
    {
        public GameObject hoverObj;
        public InventorySlot hoverSlot;
    }
}


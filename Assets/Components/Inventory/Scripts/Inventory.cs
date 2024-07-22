using System.Collections.Generic;
using InventoryScriptableObjectSpace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

                var slot = _inventorySO.Container.Items[i];

                SetSlotData(cellObj, slot);
                
                _itemsDisplayed.Add(cellObj, slot);
            }
        }

        private void SetSlotData(GameObject slotObj, InventorySlot slot)
        {
            var imageSprite = slotObj.transform.GetChild(0).GetComponentInChildren<Image>();
            imageSprite.enabled = !slot.IsEmpty;
                    
            if (!slot.IsEmpty)
            {
                imageSprite.sprite = _inventorySO.GetSpriteByItemId(slot.Item.Id);
                slotObj.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = slot.Amount == 1 ? "" : slot.Amount.ToString();
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
            
            cellObj.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

            if (_itemsDisplayed.TryGetValue(cellObj, out var slot))
            {
                _mouseItem.hoverSlot = slot;
            }
        }
        
        private void OnExit(GameObject cellObj)
        {
            cellObj.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
            
            _mouseItem.hoverObj = null;
            _mouseItem.hoverSlot = null;
        }
        
        private void OnDragBegin(GameObject cellObj)
        {
            if (_itemsDisplayed.TryGetValue(cellObj, out var slot))
            {
                if (slot.IsEmpty)
                {
                    return;
                }
                
                var mouseObject = new GameObject();
                mouseObject.transform.SetParent(transform);
            
                var rectTransform = mouseObject.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(40, 40);
                
                var draggedImage = mouseObject.AddComponent<Image>();
                draggedImage.sprite = _inventorySO.GetSpriteByItemId(slot.Item.Id);
                draggedImage.raycastTarget = false;

                _mouseItem.draggedObj = mouseObject;
                _mouseItem.draggedSlot = slot;
            }
        }
        
        private void OnDragEnd(GameObject cellObj)
        {
            if (_itemsDisplayed.TryGetValue(cellObj, out var slot))
            {
                if (slot.IsEmpty)
                {
                    return;
                }
                
                if (_mouseItem.draggedObj)
                {
                    _inventorySO.MoveItem(slot, _mouseItem.draggedSlot);
                    
                    Destroy(_mouseItem.draggedObj);
                    
                    _mouseItem.draggedObj = null;
                    _mouseItem.draggedSlot = null;
                }
                else
                {
                    _inventorySO.RemoveItem(slot);
                }
            }
        }
        
        private void OnDragged(GameObject cellObj)
        {
            if (_mouseItem.draggedObj)
            {
                _mouseItem.draggedObj.GetComponent<RectTransform>().position = Input.mousePosition;
            }
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
        public GameObject draggedObj;
        public InventorySlot draggedSlot;
        
        public GameObject hoverObj;
        public InventorySlot hoverSlot;
    }
}


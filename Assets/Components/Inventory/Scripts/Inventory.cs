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
        private Canvas _parentCanvas;

        private void Start()
        {
            _mouseItem = new MouseItem();
            _parentCanvas = transform.parent.GetComponent<Canvas>();
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
                AddEvent(cellObj, EventTriggerType.EndDrag, delegate { OnDragEnd(); });
                AddEvent(cellObj, EventTriggerType.Drag, delegate { OnDragged(); });

                var slot = _inventorySO.Container.Items[i];

                SetSlotData(cellObj, slot);
                
                _itemsDisplayed.Add(cellObj, slot);
            }
        }
        
        private void UpdateSlots()
        {
            foreach (var itemDisplayed in _itemsDisplayed)
            {
                SetSlotData(itemDisplayed.Key, itemDisplayed.Value);
            }
        }

        private void SetSlotData(GameObject slotObj, InventorySlot slot)
        {
            var imageSprite = slotObj.transform.GetChild(0).GetComponentInChildren<Image>();
            imageSprite.enabled = !slot.IsEmpty;
            imageSprite.sprite = slot.IsEmpty ? null : _inventorySO.GetSpriteByItemId(slot.Item.Id);
            
            slotObj.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = slot.Amount <= 1 ? "" : slot.Amount.ToString();
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
            cellObj.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

            _mouseItem.hoverObj = cellObj;
        }
        
        private void OnExit(GameObject cellObj)
        {
            cellObj.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
            
            _mouseItem.hoverObj = null;
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
                rectTransform.sizeDelta = new Vector2(45, 45);
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.identity;
                
                var draggedImage = mouseObject.AddComponent<Image>();
                draggedImage.sprite = _inventorySO.GetSpriteByItemId(slot.Item.Id);
                draggedImage.raycastTarget = false;
                
                _mouseItem.draggedObj = mouseObject;
                _mouseItem.draggedSlot = slot;
            }
        }
        
        private void OnDragEnd()
        {
            if (_mouseItem.hoverObj && _itemsDisplayed.TryGetValue(_mouseItem.hoverObj, out var targetSlot))
            {
                if (_mouseItem.draggedSlot != null)
                {
                    _inventorySO.MoveItem(_mouseItem.draggedSlot, targetSlot);
                    UpdateSlots();
                }
            }
            else if(_mouseItem.hoverObj == null && _mouseItem.draggedSlot != null)
            {
                _inventorySO.RemoveItem(_mouseItem.draggedSlot.Item);
                UpdateSlots();
            }
            
            Destroy(_mouseItem.draggedObj);
            _mouseItem.draggedObj = null;
            _mouseItem.draggedSlot = null;
        }
        
        private void OnDragged()
        {
            if (_mouseItem.draggedObj)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _parentCanvas.transform as RectTransform, 
                    Input.mousePosition, 
                    Camera.main, 
                    out Vector2 pos);
                
                _mouseItem.draggedObj.transform.position = _parentCanvas.transform.TransformPoint(pos);
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
        public GameObject hoverObj;
        
        public GameObject draggedObj;
        public InventorySlot draggedSlot;
    }
}


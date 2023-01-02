using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemInventoryPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _draggingParent;
    private Transform _originalParent;

    private HashSet<Transform> _parents;

    private InventoryUI _inventory;

    public BaseItemInfo _itemInfo;

    private void Start()
    {
        _inventory = FindObjectOfType<InventoryUI>();
        _draggingParent = _inventory.transform;
        _originalParent = transform.parent;

        _parents = new HashSet<Transform> 
        {
            _inventory.BackpackAreaUI, 
            _inventory.ArmorAreaUI, 
            _inventory.ExternalAreaUI, 
            _inventory.WeaponUI
        };
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Transform newParent = FindParent();
        if (_parents.Contains(newParent))
        {
            IInventoryUI originalComponent = _originalParent.GetComponent<IInventoryUI>();
            IInventoryUI newComponent = newParent.GetComponent<IInventoryUI>();
            if(_inventory.WeaponUI == newParent && _itemInfo is WeaponInfo)
            {
                if(!_inventory.TryEquipWeapon((WeaponInfo)_itemInfo, _originalParent))
                {
                    transform.SetParent(_originalParent);
                }
            }
            else
            {
                if (originalComponent.TryInteract(_itemInfo, newComponent))
                {
                    transform.SetParent(newParent);
                    _originalParent = newParent;
                }
                else
                {
                    transform.SetParent(_originalParent);
                }
            }
        }
        else
        {
            transform.SetParent(_originalParent);
        }
    }

    private Transform FindParent()
    {
        foreach (Transform parent in _parents)
            if (In(parent))
                return parent;

        return null;
    }

    private bool In(Transform rectForCheck)
    {
        return RectTransformUtility.RectangleContainsScreenPoint((RectTransform)rectForCheck, transform.position);
    }
}

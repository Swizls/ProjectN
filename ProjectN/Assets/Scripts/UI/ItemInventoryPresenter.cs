using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemInventoryPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _draggingParent;
    private Transform _originalParent;

    private InventoryUI _inventory;

    public BaseItemInfo _itemInfo;

    private void Start()
    {
        _inventory = FindObjectOfType<InventoryUI>();
        _draggingParent = _inventory.transform;
        _originalParent = transform.parent;
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
        //if (In(_originalParent))
        //{
        //    transform.SetParent(_originalParent);
        //}
        //else if (In(_inventory.BackpackAreaUI))
        //{
        //    if (_originalParent == _inventory.ExternalAreaUI)
        //    {
        //        if (PlayerUnitHandler.CurrentSelectedUnit.Inventory.Backpack.TryToAddItem(_itemInfo))
        //        {
        //            transform.SetParent(_inventory.BackpackAreaUI);
        //        }
        //        else
        //        {
        //            transform.SetParent(_originalParent);
        //        }
        //    }
        //    else
        //    {
        //        if (PlayerUnitHandler.CurrentSelectedUnit.Inventory.Armor.TryToTransitItem(_itemInfo, PlayerUnitHandler.CurrentSelectedUnit.Inventory.Backpack))
        //        {
        //            transform.SetParent(_inventory.BackpackAreaUI);
        //        }
        //        else
        //        {
        //            transform.SetParent(_originalParent);
        //        }
        //    }
        //}
        //else if (In(_inventory.ArmorAreaUI))
        //{
        //    if (_originalParent == _inventory.ExternalAreaUI)
        //    {
        //        if (PlayerUnitHandler.CurrentSelectedUnit.Inventory.Armor.TryToAddItem(_itemInfo))
        //        {
        //            transform.SetParent(_inventory.ArmorAreaUI);
        //        }
        //        else
        //        {
        //            transform.SetParent(_originalParent);
        //        }
        //    }
        //    else
        //    {
        //        if (PlayerUnitHandler.CurrentSelectedUnit.Inventory.Backpack.TryToTransitItem(_itemInfo, PlayerUnitHandler.CurrentSelectedUnit.Inventory.Armor))
        //        {
        //            transform.SetParent(_inventory.ArmorAreaUI);
        //        }
        //        else
        //        {
        //            transform.SetParent(_originalParent);
        //        }
        //    }
        //}
        //else
        //{
        //    IStorableItem container;
        //    if (_originalParent == _inventory.BackpackAreaUI)
        //        container = PlayerUnitHandler.CurrentSelectedUnit.Inventory.Backpack;
        //    else
        //        container = PlayerUnitHandler.CurrentSelectedUnit.Inventory.Armor;
        //    Eject(container);
        //}

        //if(_originalParent == _inventory.ExternalAreaUI)
        //{

        //}
    }
    private bool In(Transform rectForCheck)
    {
        return RectTransformUtility.RectangleContainsScreenPoint((RectTransform)rectForCheck, transform.position);
    }
    private void Eject(IStorableItem container)
    {
        Destroy(gameObject);
        container.RemoveItem(_itemInfo);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        if (In(_inventory.BackpackAreaUI))
        {
            if(_inventory.AllItemsInInventory.Contains(_itemInfo))
            {
                //PlayerUnitHandler.CurrentSelectedUnit.Inventory.TryToPickupToBackpack(_itemInfo);
            }
            else
            {
                //PlayerUnitHandler.CurrentSelectedUnit.Inventory.TryToTransitItemToBackpack(_itemInfo);
            }
            
            transform.SetParent(_inventory.BackpackAreaUI);
        }
        else if (In(_inventory.ArmorAreaUI))
        {
            //PlayerUnitHandler.CurrentSelectedUnit.Inventory.TryTransitToArmor(_itemInfo);
            transform.SetParent(_inventory.ArmorAreaUI);
        }
        else
        {
            Eject();
        }
    }
    private bool In(Transform rectForCheck)
    {
        return RectTransformUtility.RectangleContainsScreenPoint((RectTransform)rectForCheck, transform.position);
    }
    private void Eject()
    {
        Destroy(gameObject);
        //PlayerUnitHandler.CurrentSelectedUnit.Inventory.DropItem(_itemInfo);
    }
}

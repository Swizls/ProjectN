using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventoryPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _draggingParent;
    private Transform _originalParent;

    private InventoryUI _inventory;

    public IItemInfo _itemInfo;

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
        if(In(_originalParent))
        {
            transform.SetParent(_originalParent);
        }
        else if (In(_inventory.ArmorAreaUI))
        {
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
        PlayerUnitHandler.CurrentSelectedUnit.Inventory.RemoveItem(_itemInfo);
    }
}

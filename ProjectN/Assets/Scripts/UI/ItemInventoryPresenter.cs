using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemInventoryPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _draggingParent;
    private Transform _originalParent;

    private HashSet<Transform> _allPossibleParents;

    private InventoryUI _inventory;

    public BaseItemInfo _itemInfo;

    private void Start()
    {
        _inventory = FindObjectOfType<InventoryUI>();
        _draggingParent = _inventory.transform;
        _originalParent = transform.parent;

        _allPossibleParents = new HashSet<Transform> {_inventory.BackpackAreaUI, _inventory.ArmorAreaUI, _inventory.ExternalAreaUI};
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
        Transform _newParent = FindParent();
        if (In(_originalParent) || _newParent == null)
        {
            transform.SetParent(_originalParent);
        }
        else
        {
            if (_inventory.TryToTransitItem(_itemInfo, _originalParent, _newParent))
            {
                transform.SetParent(_newParent);
                _originalParent = _newParent;
            }
            else
            {
                transform.SetParent(_originalParent);
            }
        }
    }

    private Transform FindParent()
    {
        foreach (Transform parent in _allPossibleParents)
            if (In(parent))
                return parent;

        return null;
    }

    private bool In(Transform rectForCheck)
    {
        return RectTransformUtility.RectangleContainsScreenPoint((RectTransform)rectForCheck, transform.position);
    }
}

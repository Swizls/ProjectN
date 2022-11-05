using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventoryPresenter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _draggingParent;
    private Transform _originalParent;

    private void Start()
    {
        _draggingParent = FindObjectOfType<InventoryUI>().transform;
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
        transform.SetParent(_originalParent);
    }
}

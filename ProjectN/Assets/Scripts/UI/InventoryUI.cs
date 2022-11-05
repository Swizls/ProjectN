using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItemPrefab;
    [SerializeField] private Transform _backpackAreaUI;

    private List<ItemInfo> _itemsInBackpack = new();
    private List<GameObject> _renderedItems = new();
    private ItemInfo _currentWeapon;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Render();
    }
    private void OnDisable()
    {
        if (_renderedItems.Count > 0)
        {
            foreach (GameObject item in _renderedItems)
            {
                Destroy(item);
            }
            _renderedItems.Clear();
        }
    }
    private void Render()
    {
        _itemsInBackpack = PlayerUnitHandler.CurrentSelectedUnit.Inventory.Items;
        for(int i = 0; i < _itemsInBackpack.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _backpackAreaUI);
            createdItem.name = _itemsInBackpack[i].Name;
            createdItem.GetComponent<Image>().sprite = _itemsInBackpack[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private Transform _backpackAreaUI;
    [SerializeField] private Transform _armorAreaUI;
    [SerializeField] private Transform _externalAreaUI;

    [SerializeField] private Transform _currentWeaponUI;

    private List<GameObject> _renderedItems = new();

    public Transform BackpackAreaUI => _backpackAreaUI;
    public Transform ArmorAreaUI => _armorAreaUI;
    public Transform ExternalAreaUI => _externalAreaUI;

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
        RenderBackpack();
        RenderArmor();
    }

    private void RenderArmor()
    {
        List<BaseItemInfo> _itemsInArmor = PlayerUnitHandler.CurrentSelectedUnit.Inventory.ItemsInArmor;
        for (int i = 0; i < _itemsInArmor.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _armorAreaUI);
            createdItem.name = _itemsInArmor[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = _itemsInArmor[i];
            createdItem.GetComponent<Image>().sprite = _itemsInArmor[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }

    private void RenderBackpack()
    {
        List<BaseItemInfo> _itemsInBackpack = PlayerUnitHandler.CurrentSelectedUnit.Inventory.ItemsInBackpack;
        for (int i = 0; i < _itemsInBackpack.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, _backpackAreaUI);
            createdItem.name = _itemsInBackpack[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = _itemsInBackpack[i];
            createdItem.GetComponent<Image>().sprite = _itemsInBackpack[i].Sprite;
            _renderedItems.Add(createdItem);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItemPrefab;

    [SerializeField] private Transform _backpackAreaUI;
    [SerializeField] private Transform _armorAreaUI;
    [SerializeField] private Transform _externalAreaUI;
    [SerializeField] private Transform _currentWeaponUI;

    private List<BaseItemInfo> _itemsInArmor = new();
    private List<BaseItemInfo> _itemsInBackpack = new();
    private List<BaseItemInfo> _externalItems = new();

    private List<GameObject> _renderedItems = new();

    private UnitInventory _unitInventory;

    public Transform BackpackAreaUI => _backpackAreaUI;
    public Transform ArmorAreaUI => _armorAreaUI;
    public Transform ExternalAreaUI => _externalAreaUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(PlayerUnitHandler.CurrentSelectedUnit != null)
        {
            _unitInventory = PlayerUnitHandler.CurrentSelectedUnit.Inventory;
            Render();
        }
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
    private void UpdateItemLists()
    {
        _itemsInBackpack = _unitInventory.Backpack.StoredItems;
        _itemsInArmor = _unitInventory.Armor.StoredItems;
        _externalItems = _unitInventory.ExternalItems;
    }
    private void Render()
    {
        UpdateItemLists();

        RenderItems(_armorAreaUI, _itemsInArmor);
        RenderItems(_backpackAreaUI, _itemsInBackpack);
        RenderItems(_externalAreaUI, _externalItems);
    }

    private void RenderItems(Transform area, List<BaseItemInfo> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject createdItem = Instantiate(_inventoryItemPrefab, area);

            createdItem.name = items[i].Name;
            createdItem.GetComponent<ItemInventoryPresenter>()._itemInfo = items[i];
            createdItem.GetComponent<Image>().sprite = items[i].Sprite;

            _renderedItems.Add(createdItem);
        }
    }
    public bool TryToTransitItem(BaseItemInfo item, Transform from, Transform to)
    {
        if(from == _externalAreaUI)
        {
            if(to == _backpackAreaUI)
            {
                bool result = _unitInventory.Backpack.TryToAdd(item);
                if (result)
                {
                    _unitInventory.Pickup(item);
                }
                return result;
            }
            else
            {
                bool result = _unitInventory.Armor.TryToAdd(item);
                if (result)
                {
                    _unitInventory.Pickup(item);
                }
                return result;
            }
        }
        else if((from == _backpackAreaUI || from == _armorAreaUI) && to == _externalAreaUI)
        {
            if(from == _backpackAreaUI)
            {
                _unitInventory.Backpack.Remove(item);
            }
            else
            {
                _unitInventory.Armor.Remove(item);
            }
            return true;
        }
        else if(from == _backpackAreaUI)
        {
            return _unitInventory.Backpack.TryToTransit(item, _unitInventory.Armor);
        }
        else if(from == _armorAreaUI)
        {
            return _unitInventory.Armor.TryToTransit(item, _unitInventory.Backpack);
        }
        return false;
    }
}

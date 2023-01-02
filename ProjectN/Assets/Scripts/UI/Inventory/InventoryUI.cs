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
    [SerializeField] private Transform _weaponUI;

    private List<GameObject> _renderedItems = new();

    private UnitInventory _unitInventory;

    public Transform BackpackAreaUI => _backpackAreaUI;
    public Transform ArmorAreaUI => _armorAreaUI;
    public Transform ExternalAreaUI => _externalAreaUI;
    public Transform WeaponUI => _weaponUI;

    private void OnEnable()
    {
        if(PlayerUnitControl.Instance != null)
        {
            _unitInventory = PlayerUnitControl.Instance.CurrentUnit.Inventory;
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
    private void Render()
    {
        RenderItems(_armorAreaUI, _unitInventory.Armor.StoredItems);
        RenderItems(_backpackAreaUI, _unitInventory.Backpack.StoredItems);
        RenderItems(_externalAreaUI, _unitInventory.ExternalItems);
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
    public bool TryEquipWeapon(WeaponInfo _weaponInfo, Transform from)
    {
        _unitInventory.Weapon = new(_weaponInfo);
        return false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    private const float MAX_PICKUP_RADIUS = 1.5f;

    [SerializeField] private BackpackInfo _backpackInfo;
    [SerializeField] private ArmorInfo _armorInfo;
    [SerializeField] private WeaponInfo _weaponInfo;

    private Storable _backpack;
    private Storable _armor;

    private List<BaseItemInfo> _externalItems = new();

    private List<ItemScenePresenter> _itemPresenters;

    private bool _isStorageOpened = false;

    public Weapon Weapon;

    [HideInInspector] public LootCrate CurrentOpenedStorage;

    public bool IsStorageOpened => _isStorageOpened;
    public Storable Backpack => _backpack;
    public Storable Armor => _armor;
    public List<BaseItemInfo> ExternalItems => _externalItems;

    private void Start()
    {
        if(_weaponInfo != null)
        {
            Weapon = new(_weaponInfo);
        }
        if(_backpackInfo != null)
        {
            _backpack = new(_backpackInfo);
        }
        if(_armorInfo != null)
        {
            if (_armorInfo.GetType() == typeof(StorableArmorInfo))
            {
                StorableArmorInfo storableArmorInfo = (StorableArmorInfo)_armorInfo;
                _armor = new(storableArmorInfo);
            }
        }
    }
    public void ItemRemove(BaseItemInfo item)
    {
        if (_backpack.StoredItems.Contains(item))
            _backpack.StoredItems.Remove(item);

        if (_armor.StoredItems.Contains(item))
            _armor.StoredItems.Remove(item);
    }

    public void PickupFromGround(BaseItemInfo itemInfo)
    {
        foreach(ItemScenePresenter item in _itemPresenters)
        {
            if(item.Info == itemInfo)
            {
                item.Pickup();
            }
        }
    }
    
    public bool IsItemInInventory(BaseItemInfo item)
    {
        return _backpack.StoredItems.Contains(item) || _armor.StoredItems.Contains(item);
    }

    public bool IsHealItemInInventory(out MedicineInfo healItem)
    {
        foreach (BaseItemInfo item in _backpack.StoredItems)
        {
            if(item is MedicineInfo)
            {
                healItem = (MedicineInfo)item;
                return true;
            }
        }
        foreach (BaseItemInfo item in _armor.StoredItems)
        {
            if (item is MedicineInfo)
            {
                healItem = (MedicineInfo)item;
                return true;
            }
        }
        healItem = null;
        return false;
    }

    public void PickupFromStorage(BaseItemInfo itemInfo)
    {
        CurrentOpenedStorage.RemoveItem(itemInfo);
    }

    public void GetItemsFromStorage(LootCrate storage)
    {
        _isStorageOpened = true;
        CurrentOpenedStorage = storage;
        _externalItems = storage.Items;
    }

    public BaseItemInfo[] GetItemsOnGround()
    {
        _isStorageOpened = false; 
        _itemPresenters = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), MAX_PICKUP_RADIUS).
                                       GetComponents<ItemScenePresenter>().ToList();
        List<BaseItemInfo> itemsInfo = new();
        foreach(ItemScenePresenter item in _itemPresenters)
        {
            itemsInfo.Add(item.Info);
        }
        return itemsInfo.ToArray();
    }
}

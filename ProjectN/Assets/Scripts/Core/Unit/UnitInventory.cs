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

    private List<ItemScenePresenter> _itemPresenters;

    public Storable Backpack => _backpack;
    public Storable Armor => _armor;
    public List<BaseItemInfo> ExternalItems { get { return GetItemsOnGround().ToList(); } }
    public WeaponInfo WeaponInfo => _weaponInfo;

    private void Start()
    {
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
    public void Pickup(BaseItemInfo itemInfo)
    {
        foreach(ItemScenePresenter item in _itemPresenters)
        {
            if(item.Info == itemInfo)
            {
                item.PickedUp();
            }
        }
    }
    public BaseItemInfo[] GetItemsOnGround()
    {
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

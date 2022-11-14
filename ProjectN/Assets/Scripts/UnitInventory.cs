using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    private const float MAX_PICKUP_RADIUS = 1.5f;

    [SerializeField] private BackpackInfo _backpack;
    [SerializeField] private StorableArmorInfo _armor;
    [SerializeField] private WeaponInfo _weapon;

    public IStorableItem Backpack => _backpack;
    public IStorableItem Armor => _armor;
    public WeaponInfo Weapon => _weapon;

    public BaseItemInfo[] GetItemsOnGround()
    {

        ItemScenePresenter[] items = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), MAX_PICKUP_RADIUS).
                                                             GetComponents<ItemScenePresenter>();

        List<BaseItemInfo> itemsInfo = new();

        foreach(var item in items) 
            itemsInfo.Add(item.Info); 

        return itemsInfo.ToArray();
    }
}

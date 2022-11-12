using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    private readonly float maxCarringWeight = 50f;
    private float currentCarringWeight = 0f;

    [SerializeField] private List<IItemInfo> items = new();

    private ItemInfo currentBackpack;
    private ItemInfo currentWeapon;

    public Action<ItemInfo> newItemAdded;

    public ItemInfo CurrentBackpack => currentBackpack;
    public ItemInfo CurrentWeapon => currentWeapon;
    public List<IItemInfo> Items => items;

    public int ItemsCount()
    {
        return items.Count;
    }
    public void AddItem(IItemInfo item)
    {
        if(currentCarringWeight + item.Weight <= maxCarringWeight)
        {
            items.Add(item);
            currentCarringWeight += item.Weight;
        }
    }
    public void RemoveItem(IItemInfo item)
    {
        items.Remove(item);
        currentCarringWeight -= item.Weight;
    }
}

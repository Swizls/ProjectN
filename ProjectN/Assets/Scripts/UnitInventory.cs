using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    private const int MAX_ITEMS_COUNT = 5;

    private readonly float maxCarringWeight = 50f;
    private float currentCarringWeight = 0f;

    [SerializeField] private List<ItemInfo> items = new();

    public Action<ItemInfo> newItemAdded;

    public int ItemsCount()
    {
        return items.Count;
    }
    public void AddItem(ItemInfo item)
    {
        if(currentCarringWeight + item.Weight <= maxCarringWeight)
        {
            items.Add(item);
            currentCarringWeight += item.Weight;
        }
        Debug.Log(currentCarringWeight);
    }
    public void RemoveItem(ItemInfo item)
    {
        items.Remove(item);
        currentCarringWeight -= item.Weight;
    }
}

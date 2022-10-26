using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new();

    public int ItemCount()
    {
        return items.Count;
    }
    public void AddItem(Item item)
    {
        items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
    public void LogInventoryInConsole()
    {
        Debug.Log(items);
    }
}

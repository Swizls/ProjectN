using System.Collections.Generic;
using UnityEngine;

public class Storable
{
    private List<BaseItemInfo> _storedItems = new();
    private float _maxCarringWeight;

    public List<BaseItemInfo> StoredItems => _storedItems;

    public Storable(IStorableItemInfo info)
    {
        _maxCarringWeight = info.MaxCarringWeight;
    }

    public bool TryToAdd(BaseItemInfo item)
    {
        if (CalculateItemsWeight() + item.Weight < _maxCarringWeight)
        {
            _storedItems.Add(item);
            return true;
        }
        return false;
    }
    public void Remove(BaseItemInfo item)
    {
        if(item != null && _storedItems.Contains(item))
            _storedItems.Remove(item);
    }
    public bool TryToTransit(BaseItemInfo item, Storable container)
    {
        if(container.TryToAdd(item))
        {
            _storedItems.Remove(item);
            Debug.Log("Item added");
            return true;
        }
        return false;
    }
    private float CalculateItemsWeight()
    {
        float result = 0;

        for (int i = 0; i < _storedItems.Count; i++)
            result += _storedItems[i].Weight;

        return result;
    }
}
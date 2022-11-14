using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storable armor", menuName = "Items/Armor/Storable armor")]
public class StorableArmorInfo : ArmorInfo, IStorableItem
{
    [SerializeField] private float _maxCarringWeight;
    private float _currentCarringWeight = 0f;
    private List<BaseItemInfo> _storedItems;

    public float MaxCarringWeight => _maxCarringWeight;
    public float CurrentCarringWeight => _currentCarringWeight;
    public List<BaseItemInfo> StoredItems => _storedItems;

    public void RemoveItem(BaseItemInfo item)
    {
        if (item == null && _storedItems.Contains(item))
        {
            _storedItems.Remove(item);
            _currentCarringWeight -= item.Weight;
        }
    }

    public void TryToAddItem(BaseItemInfo item)
    {
        if (item == null && _currentCarringWeight + item.Weight <= _maxCarringWeight)
        {
            _storedItems.Add(item);
            _currentCarringWeight += item.Weight;
        }
    }

    public void TryToTransitItem(BaseItemInfo item, IStorableItem container)
    {
        container.TryToAddItem(item);
    }
}